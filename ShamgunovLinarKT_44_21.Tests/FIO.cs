using Microsoft.EntityFrameworkCore;
using NLog.Filters;
using ShamgunovLinAR_KT_44_21.Database;
using ShamgunovLinAR_KT_44_21.Interfaces.StudentsInterfaces;
using ShamgunovLinAR_KT_44_21.Models;
using ShamgunovLinAR_KT_44_21.Filters.StudentFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShamgunovLinAR_KT_44_21.Tests
{
    public class StudentTests : IDisposable
    {
        private readonly DbContextOptions<StudentDbContext> _dbContextOptions;
        private readonly StudentDbContext _context;
        private readonly StudentService _studentService;

        public StudentTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: "student_db_lS")
                .Options;

            _context = new StudentDbContext(_dbContextOptions);
            _studentService = new StudentService(_context);

            SeedDatabase();
        }

        private void SeedDatabase() // база данных
        {
            var groups = new List<Group>
    {
        new Group { GroupName = "KT-45-21", GroupId = 1 },
        new Group { GroupName = "KT-43-21", GroupId = 2 },
        new Group { GroupName = "KT-42-21", GroupId = 3 },
        new Group { GroupName = "KT-41-21", GroupId = 4 }
    };

            _context.Set<Group>().AddRange(groups);

            var students = new List<Student>
    {
        new Student { FirstName = "a", LastName = "a", MiddleName = "a", GroupId = 1 }, 
        new Student { FirstName = "a", LastName = "a", MiddleName = "a", GroupId = 2 }, 
        new Student { FirstName = "a", LastName = "a", MiddleName = "b", GroupId = 1 }, 

    };

            _context.Set<Student>().AddRange(students);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetStudentsByFioAsync_KT4421_TwoObjects() // тест по ФИО 
        {
            // Arrange
            var filter1 = new Filters.StudentFioFilters.StudentFioFilter
            {
                FirstName = "a",
                LastName = "a",
                MiddleName = "b"
            };

            // Act
            var studentsResult1 = await _studentService.GetStudentsByFioAsync(filter1, CancellationToken.None);

            // Assert
            Assert.Equal(1, studentsResult1.Length); 
        }

        [Fact]
        public async Task GetStudentsByIdGroupAsync_KT4421_TwoObjects() // тест по GroupId 
        {
            // Act
            var filter2 = new Filters.GroupFilter.StudentIdGroup
            {
                GroupId = 1
            };

            var studentsResult2 = await _studentService.GetStudentsByIdGroupAsync(filter2, CancellationToken.None);

            // Assert
            Assert.Equal(2, studentsResult2.Length); 
        }

        [Fact]
        public async Task GetStudentsByGroupNameAsync_KT4421_TwoObjects() // тест по GroupName
        {
            // Arrange
            var groupNameFilter = new Filters.StudentFilters.StudentGroupFilter
            {
                GroupName = "KT-45-21"
            };

            // Act
            var studentsResult = await _studentService.GetStudentsByGroupAsync(groupNameFilter, CancellationToken.None);

            // Assert
            Assert.Equal(2, studentsResult.Length); 
        }


        public void Dispose()
        {
            // Удаление базы данных после тестов
            using (var context = new StudentDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
