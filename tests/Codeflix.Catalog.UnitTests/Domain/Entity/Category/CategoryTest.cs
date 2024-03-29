﻿using Codeflix.Catalog.Domain.Exceptions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using Xunit;
using DomainEntity = Codeflix.Catalog.Domain.Entity;

namespace Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            // Arrange
            var validData = new
            {
                Name = "category name",
                Description = "caregory description"
            };

            var dateTimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description);
            var dateTimeAfter = DateTime.Now;
            // Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > dateTimeBefore);
            Assert.True(category.CreatedAt < dateTimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            // Arrange
            var validData = new
            {
                Name = "category name",
                Description = "caregory description"
            };

            var dateTimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
            var dateTimeAfter = DateTime.Now;
            // Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > dateTimeBefore);
            Assert.True(category.CreatedAt < dateTimeAfter);
            Assert.Equal(isActive, category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            Action action = 
                () => new DomainEntity.Category(name!, "Category Description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            Action action =
                () => new DomainEntity.Category("Category Name", null!);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should not be null", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ca")]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            Action action =
                () => new DomainEntity.Category(invalidName, "Category OK Description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            Action action =
                () => new DomainEntity.Category(invalidName, "Category OK Description");
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan10_000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
            Action action =
                () => new DomainEntity.Category("Category Name", invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "caregory description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, false);
            category.Activate();
          
            Assert.True( category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validData = new
            {
                Name = "category name",
                Description = "caregory description"
            };

            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Deactivate();

            Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name", Description = "New Description" };

            category.Update(newValues.Name, newValues.Description);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name" };
            var currentDescription = category.Description;

            category.Update(newValues.Name);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(currentDescription, category.Description);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            Action action =
                () => category.Update(name!);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ca")]
        public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            Action action =
                () => category.Update(invalidName);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var category = new DomainEntity.Category("Category Name", "Category Description");
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            Action action =
                () => category.Update(invalidName);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan10_000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
            var category = new DomainEntity.Category("Category Name", "Category Description");
            Action action =
                () => category.Update("Category New Name", invalidDescription);
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }
    }
}
