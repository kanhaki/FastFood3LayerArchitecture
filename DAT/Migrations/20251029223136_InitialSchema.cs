using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAT.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactID);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "DroneStation",
                columns: table => new
                {
                    StationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Location_Lat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Location_Lng = table.Column<decimal>(type: "decimal(9,6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DroneStation", x => x.StationID);
                });

            migrationBuilder.CreateTable(
                name: "DroneStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DroneStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "FoodStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactionStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactionStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Drone",
                columns: table => new
                {
                    DroneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationID = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentBattery = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MaxLoad = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CurrentLocation_Lat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    CurrentLocation_Lng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drone", x => x.DroneID);
                    table.ForeignKey(
                        name: "FK_Drone_DroneStation_StationID",
                        column: x => x.StationID,
                        principalTable: "DroneStation",
                        principalColumn: "StationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Drone_DroneStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "DroneStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodItem",
                columns: table => new
                {
                    FoodID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    FoodName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItem", x => x.FoodID);
                    table.ForeignKey(
                        name: "FK_FoodItem_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodItem_FoodStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "FoodStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    AvatarURL = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_User_UserRole_RoleID",
                        column: x => x.RoleID,
                        principalTable: "UserRole",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AdrsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdrsCustomerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AdrsLine = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AdrsID);
                    table.ForeignKey(
                        name: "FK_Address_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    RestaurantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OpeningHours = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location_Lat = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Location_Lng = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.RestaurantID);
                    table.ForeignKey(
                        name: "FK_Restaurant_RestaurantStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "RestaurantStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurant_User_ManagerID",
                        column: x => x.ManagerID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    AdrsID = table.Column<int>(type: "int", nullable: false),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_Address_AdrsID",
                        column: x => x.AdrsID,
                        principalTable: "Address",
                        principalColumn: "AdrsID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_OrderStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "OrderStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Restaurant_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurant",
                        principalColumn: "RestaurantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    DeliveryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    DroneID = table.Column<int>(type: "int", nullable: false),
                    EstimatedPickupTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualPickupTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedDropoffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDropoffTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.DeliveryID);
                    table.ForeignKey(
                        name: "FK_Delivery_DeliveryStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "DeliveryStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Delivery_Drone_DroneID",
                        column: x => x.DroneID,
                        principalTable: "Drone",
                        principalColumn: "DroneID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Delivery_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    FoodID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => new { x.OrderID, x.FoodID });
                    table.ForeignKey(
                        name: "FK_OrderItem_FoodItem_FoodID",
                        column: x => x.FoodID,
                        principalTable: "FoodItem",
                        principalColumn: "FoodID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransaction",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vnp_TransactionNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OrderInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponseCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransaction", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_PaymentTransaction_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentTransaction_PaymentTransactionStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "PaymentTransactionStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserID",
                table: "Address",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_Email",
                table: "Contact",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_DroneID",
                table: "Delivery",
                column: "DroneID");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderID",
                table: "Delivery",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_StatusID",
                table: "Delivery",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Drone_StationID",
                table: "Drone",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Drone_StatusID",
                table: "Drone",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItem_CategoryID",
                table: "FoodItem",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItem_StatusID",
                table: "FoodItem",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AdrsID",
                table: "Order",
                column: "AdrsID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_RestaurantID",
                table: "Order",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StatusID",
                table: "Order",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserID",
                table: "Order",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_FoodID",
                table: "OrderItem",
                column: "FoodID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransaction_OrderID",
                table: "PaymentTransaction",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransaction_StatusID",
                table: "PaymentTransaction",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_ManagerID",
                table: "Restaurant",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_StatusID",
                table: "Restaurant",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "PaymentTransaction");

            migrationBuilder.DropTable(
                name: "DeliveryStatus");

            migrationBuilder.DropTable(
                name: "Drone");

            migrationBuilder.DropTable(
                name: "FoodItem");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "PaymentTransactionStatus");

            migrationBuilder.DropTable(
                name: "DroneStation");

            migrationBuilder.DropTable(
                name: "DroneStatus");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "FoodStatus");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "RestaurantStatus");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserRole");*/
        }
    }
}
