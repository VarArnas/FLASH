﻿// <auto-generated />
using System;
using FirstLab.src.back_end.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FirstLab.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231017082344_FlashcardSetLog#2")]
    partial class FlashcardSetLog2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("FirstLab.Flashcard", b =>
                {
                    b.Property<string>("FlashcardName")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(1);

                    b.Property<string>("FlashcardSetName")
                        .HasColumnType("TEXT")
                        .HasColumnOrder(2);

                    b.Property<string>("FlashcardAnswer")
                        .HasColumnType("TEXT");

                    b.Property<string>("FlashcardColor")
                        .HasColumnType("TEXT");

                    b.Property<string>("FlashcardQuestion")
                        .HasColumnType("TEXT");

                    b.HasKey("FlashcardName", "FlashcardSetName");

                    b.HasIndex("FlashcardSetName");

                    b.ToTable("Flashcards");
                });

            modelBuilder.Entity("FirstLab.FlashcardSet", b =>
                {
                    b.Property<string>("FlashcardSetName")
                        .HasColumnType("TEXT");

                    b.HasKey("FlashcardSetName");

                    b.ToTable("FlashcardSets");
                });

            modelBuilder.Entity("FirstLab.src.back_end.FlashcardSetLog", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayedSetsName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Date");

                    b.ToTable("FlashcardsLog");
                });

            modelBuilder.Entity("FirstLab.Flashcard", b =>
                {
                    b.HasOne("FirstLab.FlashcardSet", null)
                        .WithMany("Flashcards")
                        .HasForeignKey("FlashcardSetName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FirstLab.FlashcardSet", b =>
                {
                    b.Navigation("Flashcards");
                });
#pragma warning restore 612, 618
        }
    }
}
