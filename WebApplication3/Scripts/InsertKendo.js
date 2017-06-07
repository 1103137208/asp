
$(document).ready(function () {
    $(function () {
        $.getScript("../Scripts/kendo.all.min.js", function () {

            //取得商品價格資料
            var Product;
            $.getJSON("/Order/GetProductPrice", function (data) {
                Product = data;
            });

            //取得商品資料
            var ProductDataSource = new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "../Order/GetProductList",
                        dataType: "json"
                    }
                }, schema: {
                    model: {
                        fields: {
                            Value: { type: "string" },
                            Text: { type: "string" },
                        }
                    }
                },
                pageSize: 80,
                serverPaging: true,
                serverFiltering: true
            });


            //客戶名稱
            $("#CustomerID").kendoComboBox({
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: {
                    transport: {
                        read: "../Order/GetCustomerList",
                        dataType: "json",
                    },
                    schema: {
                        model: {
                            fields: {
                                Value: { type: "string" },
                                Text: { type: "string" },
                            }
                        }
                    },
                    pageSize: 80,
                    serverPaging: true,
                    serverFiltering: true
                }
            });

            //員工名稱
            $("#EmployeeID").kendoComboBox({
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: {
                    transport: {
                        read: "../Order/GetEmployeeList",
                        dataType: "json",
                    },
                    schema: {
                        model: {
                            fields: {
                                Value: { type: "string" },
                                Text: { type: "string" },
                            }
                        }
                    },
                    pageSize: 80,
                    serverPaging: true,
                    serverFiltering: true
                }
            });

            //出貨公司
            $("#ShipperID").kendoComboBox({
                dataTextField: "Text",
                dataValueField: "Value",
                dataSource: {
                    transport: {
                        read: "../Order/GetShipperList",
                        dataType: "json",
                    },
                    schema: {
                        model: {
                            fields: {
                                Value: { type: "string" },
                                Text: { type: "string" },
                            }
                        }
                    },
                    pageSize: 80,
                    serverPaging: true,
                    serverFiltering: true
                }
            });

            //日期
            $("#OrderDate").kendoDatePicker({
                format: "yyyy/MM/dd",
                parseFormats:["yyyy/MM/dd"]
            });
            $("#ShippedDate").kendoDatePicker({
                format: "yyyy/MM/dd",
                parseFormats: ["yyyy/MM/dd"]
            });
            $("#RequireDdate").kendoDatePicker({
                format: "yyyy/MM/dd",
                parseFormats:["yyyy/MM/dd"]
            });

            $("#BackBtn").kendoButton({
                click: function () {
                    window.location.href = 'Index';
                }
            })

            //存檔按鈕(送出表單)
            $("#InsertBtn").kendoButton({
                click: function () {
                    SetOrderDetials();
                    alert('click');
                    var validator = $("#Form").data("kendoValidator");
                    if (validator.validate()) {
                        $.ajax({
                            type: "POST",
                            url: "../Order/DoInsertOrder",
                            data: $("#Form").serialize(),
                            success: function (response) {
                                alert("Insert Success! OrderID : " + response);
                                window.location.href = 'Index';
                            }
                        });
                    }
                }
            })

            //表單送出前置換element name 讓Action 可以讀到OrderDetials
            function SetOrderDetials() {
                for (var i = 0 ; i < $('#tbContent tr').length; i++) {
                    $('input[itype="ProductID"]').eq(i).attr('name', 'OrderDetail[' + i + '].ProductId');
                    $('input[itype="UnitPrice"]').eq(i).attr('name', 'OrderDetail[' + i + '].UnitPrice');
                    $('input[itype="Qty"]').eq(i).attr('name', 'OrderDetail[' + i + '].Qty');
                }
            }

            //明細資料範本
            var trtemplate = $('#tbContent tr').eq(0).html();

            //新增一列名資料
            $("#addNewDetial").kendoButton({
                click: function () {
                    $('#tbContent').append('<tr>' + trtemplate + '</tr>');
                    SetNewRow();
                    $('input[itype="ProductID"]').change(function () {
                        var productid = $(this);
                        ProductIdChange(productid);
                    });

                    $('input[itype="Qty"]').change(function () {
                        var Qty = $(this);
                        QtyChange(Qty);
                    });
                }
            })

            SetNewRow();

            //將kendo套到新的資料列
            function SetNewRow() {
                $('input[itype="ProductID"]').kendoComboBox({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    dataSource: ProductDataSource
                });

                //移除TR
                $('.btnRemove').kendoButton({
                    click: RemoveRow
                });

                $('input[itype="Qty"]').kendoNumericTextBox();
            }

            function RemoveRow(e) {
                var tr = $(e.event.target.closest('tr'));
                console.log(tr)
                if ($('#tbContent tr').length > 1) {
                    tr.remove();
                    Sum();
                } else {
                    alert("訂單資料不可少於一筆");
                }
            }

            //ProductID改變
            $('input[itype="ProductID"]').change(function () {
                var productid = $(this);
                ProductIdChange(productid);
            });

            //Qty改變
            $('input[itype="Qty"]').change(function () {
                var Qty = $(this);
                QtyChange(Qty);
            });

            //抓取新數量，算新的小計
            function QtyChange(Qty) {
                var UnitPrice = $(Qty).closest('td').prev().children('input[itype="UnitPrice"]');
                var Sub = $(Qty).closest('td').next();
                console.log(Qty)
                Subtotal($(UnitPrice).val(), $(Qty).val(), Sub);
            }

            //根據ProductID改變UnitPrice 並算新的小計
            function ProductIdChange(productid) {
                console.log(productid);
                var UnitPrice = $(productid).closest('td').next().children('input[itype="UnitPrice"]');
                var Qty = $(productid).closest('td').next().next().children().children().children('input[itype="Qty"]');
                console.log(Qty)
                var Sub = $(productid).closest('td').next().next().next();
                for (var i = 0, len = Product.length; i < len; i++) {
                    if (Product[i].ProductID == productid.val()) {
                        UnitPrice.val(Product[i].UnitPrice);
                        Subtotal(Product[i].UnitPrice, Qty.val(), Sub);
                    }
                }
            }

            //計算小計
            function Subtotal(UnitPrice, Qty, Sub) {
                console.log(UnitPrice, Qty)
                if (Qty > 0) {
                    Sub.text(UnitPrice * Qty);
                } else {
                    Sub.text(UnitPrice * 1);
                }

                Sum();
            }

            //計算總計
            function Sum() {
                var sum = 0;
                for (var i = 0 ; i < $('#tbContent tr').length; i++) {
                    if ($('.subtotal').eq(i).text() != "") {
                        sum += parseInt($('.subtotal').eq(i).text());
                    }
                    
                }
                $('#sum').text(sum);
            }

            //驗證
            var container = $("#Form");
            kendo.init(container);
            container.kendoValidator({
                rules: {
                    greaterdate: function (input) {
                        if (input.is("[data-greaterdate-msg]") && input.val() != "") {
                            var date = kendo.parseDate(input.val()),
                                otherDate = kendo.parseDate($("[name='" + input.data("greaterdateField") + "']").val());
                            return otherDate == null || otherDate.getTime() < date.getTime();
                        }

                        return true;
                    }
                }
            });

        })
    });
});