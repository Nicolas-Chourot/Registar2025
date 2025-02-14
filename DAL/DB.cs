﻿using JSON_DAL;
using System;

namespace JsonDemo.Models
{
    public sealed class DB
    {
        public static DateTime CurrentDate = new DateTime(2025, 2, 15);
        #region singleton setup
        private static readonly DB instance = new DB();
        public static DB Instance { 
            get { 
                return instance; 
            } }
        #endregion
        #region Repositories

        static public StudentsRepository Students { get; set; }
            = new StudentsRepository();

        static public CoursesRepository Courses { get; set; }
            = new CoursesRepository();

        static public TeachersRepository Teachers { get; set; }
            = new TeachersRepository();

        static public Repository<Registration> Registrations { get; set; }
            = new Repository<Registration>();

        static public Repository<Allocation> Allocations { get; set; }
            = new Repository<Allocation>();

        #endregion
    }
}