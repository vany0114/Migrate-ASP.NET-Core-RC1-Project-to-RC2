using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ParkingLot.Data;

namespace ParkingLot.Data.Migrations
{
    [DbContext(typeof(ParkingLotContext))]
    partial class ParkingLotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ParkingLot.Data.Model.ParkingLot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BikeSpaces");

                    b.Property<int>("Gates");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<int>("PrivateSpaces");

                    b.Property<int>("PublicSpaces");

                    b.HasKey("Id");

                    b.ToTable("ParkingLot");
                });
        }
    }
}
