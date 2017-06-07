
$(document).ready(function () {
    $(function () {
        $.getScript("https://kendo.cdn.telerik.com/2017.2.504/js/kendo.all.min.js", function () {

            //查詢結果grid
            $("#grid").kendoGrid({
                dataSource: {
                    transport: {
                        read: {
                            url: "../Order/Read",
                            dataType: "json",
                        },
                    },
                    pageSize: 20
                },
                columns: [{
                    field: "OrderID",
                    title: "訂單編號",
                    width: "50px"
                }, {
                    field: "CompanyName",
                    title: "客戶名稱",
                    width: "100px"
                }, {
                    field: "Orderdate",
                    title: "訂購日期",
                    width: "100px"
                }, {
                    field: "ShippedDate",
                    title: "出貨日期",
                    width: "100px"
                }, {
                    command: [{
                        name: "Update",
                        click: UpdateOrder
                    }, {
                        name: "Delete",
                        click: DeleteOrder
                    }], title: "&nbsp;", width: "100px"
                }],
                pageable: {
                    pageSizes: true,
                    buttonCount: 5
                },
                editable: false,
                sortable:true,
            });

            //員工資料
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

            //送貨公司資料
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


            //搜尋按鈕 送出表單 改變Grid資料
            $("#SearchBtn").kendoButton({
                click: function () {
                    $.ajax({
                        type: "POST",
                        url: "../Order/Read",
                        data: $("#Form").serialize(),
                        success: function (response) {
                            var dataSource = new kendo.data.DataSource({
                                data: response,
                                pageSize: 20
                            });
                            var grid = $("#grid").data("kendoGrid");
                            grid.setDataSource(dataSource);
                        }
                    });
                }

            });

            //按鈕設置
            $("#ResetBtn").kendoButton();
            $("#InsertBtn").kendoButton({
                click: function () {
                    console.log("click");
                    window.location.href = 'InsertOrder';
                }
            });

            //日期設置
            $("#OrderDate").kendoDatePicker();
            $("#ShippedDate").kendoDatePicker();
            $("#RequireDdate").kendoDatePicker();

            function UpdateOrder(e) {
                var tr = e.currentTarget.closest('tr');
                var dataItem = this.dataItem(tr);
                var orderId = dataItem.OrderID;
                console.log(orderId)
                window.location.href = 'UpdateOrder?orderId='+orderId;
            }

            //刪除資料
            function DeleteOrder(e) {
                var tr = e.currentTarget.closest('tr');
                var dataItem = this.dataItem(tr);
                console.log(dataItem.OrderID);
                $.ajax({
                    type: "POST",
                    url: "/Order/DeleteOrder",
                    data: {
                        "OrderId": dataItem.OrderID
                    },
                    dataType: "json",
                    success: function (response) {
                        if (response) {
                            alert("訂單 " + dataItem.OrderID + " 刪除成功")
                            var grid = $("#grid").data("kendoGrid");
                            grid.dataSource.remove(dataItem);
                            tr.remove();
                        } else {

                            alert("訂單 " + dataItem.OrderID + " 刪除失敗")
                        }
                    }
                });
            }
        })
    });
});