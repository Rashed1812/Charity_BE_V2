using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReconcileRequestSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReconcileRequests_AspNetUsers_UserId",
                table: "ReconcileRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReconcileRequests_UserId",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReconcileRequests");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedToMediationAt",
                table: "ReconcileRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedToSupervisorAt",
                table: "ReconcileRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "ReconcileRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConsultantNotes",
                table: "ReconcileRequests",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediationId",
                table: "ReconcileRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReconcileRequestTypeId",
                table: "ReconcileRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "ReconcileRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ReconcileRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorId",
                table: "ReconcileRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Mediations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ReconcileRequestAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReconcileRequestId = table.Column<int>(type: "int", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReconcileRequestAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReconcileRequestAttachments_ReconcileRequests_ReconcileRequestId",
                        column: x => x.ReconcileRequestId,
                        principalTable: "ReconcileRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReconcileRequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReconcileRequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supervisors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supervisors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReconcileRequests_MediationId",
                table: "ReconcileRequests",
                column: "MediationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReconcileRequests_ReconcileRequestTypeId",
                table: "ReconcileRequests",
                column: "ReconcileRequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReconcileRequests_SupervisorId",
                table: "ReconcileRequests",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReconcileRequestAttachments_ReconcileRequestId",
                table: "ReconcileRequestAttachments",
                column: "ReconcileRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Supervisors_UserId",
                table: "Supervisors",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReconcileRequests_Mediations_MediationId",
                table: "ReconcileRequests",
                column: "MediationId",
                principalTable: "Mediations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ReconcileRequests_ReconcileRequestTypes_ReconcileRequestTypeId",
                table: "ReconcileRequests",
                column: "ReconcileRequestTypeId",
                principalTable: "ReconcileRequestTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReconcileRequests_Supervisors_SupervisorId",
                table: "ReconcileRequests",
                column: "SupervisorId",
                principalTable: "Supervisors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReconcileRequests_Mediations_MediationId",
                table: "ReconcileRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReconcileRequests_ReconcileRequestTypes_ReconcileRequestTypeId",
                table: "ReconcileRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReconcileRequests_Supervisors_SupervisorId",
                table: "ReconcileRequests");

            migrationBuilder.DropTable(
                name: "ReconcileRequestAttachments");

            migrationBuilder.DropTable(
                name: "ReconcileRequestTypes");

            migrationBuilder.DropTable(
                name: "Supervisors");

            migrationBuilder.DropIndex(
                name: "IX_ReconcileRequests_MediationId",
                table: "ReconcileRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReconcileRequests_ReconcileRequestTypeId",
                table: "ReconcileRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReconcileRequests_SupervisorId",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "AssignedToMediationAt",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "AssignedToSupervisorAt",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "ConsultantNotes",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "MediationId",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "ReconcileRequestTypeId",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "ReconcileRequests");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Mediations");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReconcileRequests",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReconcileRequests_UserId",
                table: "ReconcileRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReconcileRequests_AspNetUsers_UserId",
                table: "ReconcileRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
