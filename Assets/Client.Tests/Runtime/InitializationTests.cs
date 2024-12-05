using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Core.StartUp;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Client.Tests.Runtime
{
    internal sealed class InitializationTests
    {
        private static IEnumerable<TestCaseData> GetStepTypes()
        {
            var assembly = typeof(IStep).Assembly;

            var stepTypes = assembly.GetTypes()
                .Where(t => typeof(IStep).IsAssignableFrom(t) &&
                            !t.IsInterface &&
                            !t.IsAbstract);

            foreach (var type in stepTypes)
            {
                yield return new TestCaseData(type)
                    .Returns(null)
                    .SetName($"Test_{type.Name}");
            }
        }

        [UnityTest]
        [TestCaseSource(nameof(GetStepTypes))]
        public IEnumerator InitSteps_ShouldNotThrowException(Type step)
        {
            // Arrange
            var dbStep = (IStep)Activator.CreateInstance(step);

            // Act
            var task = dbStep.Execute(0);

            while (!task.IsCompleted)
            {
                yield return null;
            }

            // Assert
            if (task.Exception != null)
            {
                Debug.LogError($"Task failed with exception: {task.Exception.InnerException?.Message}");
                Assert.Fail($"Task threw an exception: {task.Exception.InnerException?.Message}");
            }

            Assert.That(task.IsFaulted, Is.False, $"{step.Name} failed with an exception");
            Assert.That(task.IsCanceled, Is.False, $"{step.Name} was cancelled");
            Assert.That(task.IsCompletedSuccessfully, Is.True, $"{step.Name} did not complete successfully");
        }
    }
}