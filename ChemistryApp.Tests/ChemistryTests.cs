using Xunit;
using ChemistryApp.ViewModels;
using ChemistryApp.Models;
using ChemistryApp.Services;
using System.Linq;
using System.Threading.Tasks;

namespace ChemistryApp.Tests
{
    /// <summary>
    /// Набор модульных тестов для проверки логики поиска, валидации и работы с данными.
    /// </summary>
    public class ChemistryTests
    {
        /// <summary>
        /// Тест: проверяет, что поиск по символу "Fe" находит элемент "Железо".
        /// </summary>
        [Fact]
        public void Search_WithValidSymbol_ReturnsCorrectElement()
        {
            var vm = new PeriodicTableViewModel();
            vm.Elements.Clear();
            vm.Elements.Add(new ChemicalElement { AtomicNumber = 26, Symbol = "Fe", Name = "Железо", CanvasX = 0, CanvasY = 0 });
            vm.SearchText = "Fe";
            Assert.Single(vm.DisplayedElements);
            Assert.Equal("Fe", vm.DisplayedElements.First().Symbol);
        }

        /// <summary>
        /// Тест: проверяет, что ввод спецсимволов в поиск вызывает ошибку валидации.
        /// </summary>
        [Fact]
        public void Search_WithSpecialCharacters_TriggerValidationError()
        {
            var vm = new PeriodicTableViewModel();
            vm.SearchText = "Fe@#$";
            Assert.True(vm.HasErrors);
            var errors = vm.GetErrors("SearchText").Cast<string>().ToList();
            Assert.Contains(errors, e => e.Contains("Разрешены только буквы"));
        }

        /// <summary>
        /// Тест: проверяет, что из базы загружаются минимум три требуемые темы.
        /// </summary>
        [Fact]
        public async Task GetTopicsAsync_ReturnsAtLeastThreeTopics()
        {
            await DatabaseService.InitializeDatabaseAsync();
            var topics = await DatabaseService.GetTopicsAsync();
            Assert.NotNull(topics);
            Assert.True(topics.Count >= 3);
            var titles = topics.Select(t => t.Title).ToList();
            Assert.Contains("Тела и вещества", titles);
        }

        /// <summary>
        /// Тест: проверяет правильность расчёта координат для элементов таблицы.
        /// </summary>
        /// <param name="atomicNumber">Атомный номер элемента.</param>
        /// <param name="group">Номер группы.</param>
        /// <param name="expectedX">Ожидаемая координата X.</param>
        /// <param name="expectedY">Ожидаемая координата Y.</param>
        [Theory]
        [InlineData(1, 1, 0, 0)]
        [InlineData(2, 18, 731, 0)]
        public void Element_CalculatesCorrectCanvasPosition(int atomicNumber, int group, double expectedX, double expectedY)
        {
            int period = atomicNumber <= 2 ? 1 : atomicNumber <= 10 ? 2 : atomicNumber <= 18 ? 3 : atomicNumber <= 36 ? 4 : atomicNumber <= 54 ? 5 : atomicNumber <= 86 ? 6 : 7;
            const double step = 43.0;
            double canvasX = (group - 1) * step;
            double canvasY = (period - 1) * step;
            if ((period == 6 || period == 7) && group >= 3) canvasY += step * 2;
            Assert.Equal(expectedX, canvasX, precision: 0);
            Assert.Equal(expectedY, canvasY, precision: 0);
        }

        /// <summary>
        /// Тест: проверяет, что поиск несуществующего элемента показывает сообщение об ошибке.
        /// </summary>
        [Fact]
        public void Search_ForNonExistentElement_ShowsNotFoundError()
        {
            var vm = new PeriodicTableViewModel();
            vm.Elements.Clear();
            vm.Elements.Add(new ChemicalElement { Symbol = "H", Name = "Водород" });
            vm.SearchText = "Unobtainium";
            Assert.Empty(vm.DisplayedElements);
            Assert.True(vm.HasErrors);
            var errors = vm.GetErrors("SearchText").Cast<string>().ToList();
            Assert.Contains(errors, e => e.Contains("не найден"));
        }
    }
}
