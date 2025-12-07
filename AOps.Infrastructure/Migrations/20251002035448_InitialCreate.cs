using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AOps.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address_ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address_Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrimaryMobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondaryMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GST = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSite",
                columns: table => new
                {
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteZip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSite", x => x.SiteId);
                });

            migrationBuilder.CreateTable(
                name: "Employeemaster",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    EmploymentTypeId = table.Column<int>(type: "int", nullable: false),
                    GovernmentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessionalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employeemaster", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "OrganisationLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationLevels", x => x.Id);
                    table.UniqueConstraint("AK_OrganisationLevels_UserID", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "SiteExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseType = table.Column<int>(type: "int", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpenseAmount = table.Column<float>(type: "real", nullable: false),
                    ExpenseRemarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutiveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpenseReceipt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteExpenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VendorMaster",
                columns: table => new
                {
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorMobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorMaster", x => x.VendorId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerContract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AgreementDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehiclesAgreed = table.Column<int>(type: "int", nullable: false),
                    StaffAgreed = table.Column<int>(type: "int", nullable: false),
                    VehiclesUsed = table.Column<int>(type: "int", nullable: false),
                    StaffsUsed = table.Column<int>(type: "int", nullable: false),
                    ContractType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerContract_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerContract_Customers_CustomerUserId",
                        column: x => x.CustomerUserId,
                        principalTable: "Customers",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CustomerLogin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerLogin_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiteEmployeeAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteEmployeeAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteEmployeeAssignment_CustomerSite_SiteId",
                        column: x => x.SiteId,
                        principalTable: "CustomerSite",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteEmployeeAssignment_Employeemaster_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employeemaster",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AOESiteMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MappingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AOESiteMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AOESiteMapping_CustomerSite_SiteId",
                        column: x => x.SiteId,
                        principalTable: "CustomerSite",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AOESiteMapping_OrganisationLevels_OrgEmployeeId",
                        column: x => x.OrgEmployeeId,
                        principalTable: "OrganisationLevels",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleMaster",
                columns: table => new
                {
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleFuelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmbulanceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorContractId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleMaster", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_VehicleMaster_VendorMaster_VendorId",
                        column: x => x.VendorId,
                        principalTable: "VendorMaster",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorContract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AgreementDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehiclesAgreed = table.Column<int>(type: "int", nullable: false),
                    StaffAgreed = table.Column<int>(type: "int", nullable: false),
                    VehiclesUsed = table.Column<int>(type: "int", nullable: false),
                    StaffsUsed = table.Column<int>(type: "int", nullable: false),
                    ContractType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorMasterVendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorContract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorContract_VendorMaster_VendorId",
                        column: x => x.VendorId,
                        principalTable: "VendorMaster",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorContract_VendorMaster_VendorMasterVendorId",
                        column: x => x.VendorMasterVendorId,
                        principalTable: "VendorMaster",
                        principalColumn: "VendorId");
                });

            migrationBuilder.CreateTable(
                name: "CustomerTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketPriority = table.Column<int>(type: "int", nullable: false),
                    TicketStatus = table.Column<int>(type: "int", nullable: false),
                    TicketDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketCategory = table.Column<int>(type: "int", nullable: false),
                    TicketIssueType = table.Column<int>(type: "int", nullable: false),
                    ResolutionNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HandledBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExecutiveId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTickets_CustomerSite_SiteId",
                        column: x => x.SiteId,
                        principalTable: "CustomerSite",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerTickets_Employeemaster_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employeemaster",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_CustomerTickets_OrganisationLevels_ExecutiveId",
                        column: x => x.ExecutiveId,
                        principalTable: "OrganisationLevels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerTickets_VehicleMaster_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "VehicleMaster",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "SiteVehicleAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false),
                    AssignedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteVehicleAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteVehicleAssignment_CustomerSite_SiteId",
                        column: x => x.SiteId,
                        principalTable: "CustomerSite",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteVehicleAssignment_VehicleMaster_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "VehicleMaster",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleDocument_VehicleMaster_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "VehicleMaster",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AOESiteMapping_OrgEmployeeId",
                table: "AOESiteMapping",
                column: "OrgEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AOESiteMapping_SiteId",
                table: "AOESiteMapping",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContract_CustomerId",
                table: "CustomerContract",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerContract_CustomerUserId",
                table: "CustomerContract",
                column: "CustomerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLogin_CustomerId",
                table: "CustomerLogin",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_EmployeeId",
                table: "CustomerTickets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_ExecutiveId",
                table: "CustomerTickets",
                column: "ExecutiveId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_SiteId",
                table: "CustomerTickets",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_VehicleId",
                table: "CustomerTickets",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteEmployeeAssignment_EmployeeId",
                table: "SiteEmployeeAssignment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteEmployeeAssignment_SiteId",
                table: "SiteEmployeeAssignment",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteVehicleAssignment_SiteId",
                table: "SiteVehicleAssignment",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteVehicleAssignment_VehicleId",
                table: "SiteVehicleAssignment",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDocument_VehicleId",
                table: "VehicleDocument",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleMaster_VendorId",
                table: "VehicleMaster",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorContract_VendorId",
                table: "VendorContract",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorContract_VendorMasterVendorId",
                table: "VendorContract",
                column: "VendorMasterVendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AOESiteMapping");

            migrationBuilder.DropTable(
                name: "CustomerContract");

            migrationBuilder.DropTable(
                name: "CustomerLogin");

            migrationBuilder.DropTable(
                name: "CustomerTickets");

            migrationBuilder.DropTable(
                name: "SiteEmployeeAssignment");

            migrationBuilder.DropTable(
                name: "SiteExpenses");

            migrationBuilder.DropTable(
                name: "SiteVehicleAssignment");

            migrationBuilder.DropTable(
                name: "VehicleDocument");

            migrationBuilder.DropTable(
                name: "VendorContract");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "OrganisationLevels");

            migrationBuilder.DropTable(
                name: "Employeemaster");

            migrationBuilder.DropTable(
                name: "CustomerSite");

            migrationBuilder.DropTable(
                name: "VehicleMaster");

            migrationBuilder.DropTable(
                name: "VendorMaster");
        }
    }
}
