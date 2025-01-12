using System.Collections;
using Client.Scripts.Core.AI;
using Client.Scripts.Core.StartUp;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Client.Tests.Runtime
{
    [TestFixture]
    internal class AIControllerTests : Injectable
    {
        private const string TestStoryPrompt = "Write a story about a magic backpack.";
        private const string TestChatInitial = "I have 2 dogs in my house.";
        private const string TestChatFollowup = "How many paws are in my house?";

        [Inject] private IAIController _controller;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            yield return new WaitUntil(() => StartUpController.IsInited);

            yield return _controller.ClearChatHistoryAsync().WaitForTask();
        }

        [UnityTest]
        public IEnumerator SendPrompt_WithValidInput_ShouldReturnResponse()
        {
            // Arrange
            var prompt = TestStoryPrompt;

            // Act
            var task = _controller.SendPromptAsync(prompt);
            yield return task.WaitForTask();

            // Assert
            Assert.That(task.Result, Is.Not.Null, "Response should not be null");
            Assert.That(task.Result.Length, Is.GreaterThan(0), "Response should not be empty");
        }

        [UnityTest]
        public IEnumerator SendChatMessage_WithContext_ShouldMaintainConversationState()
        {
            // Arrange
            // First message establishes context
            var initialTask = _controller.SendChatMessageAsync(TestChatInitial);
            yield return initialTask.WaitForTask();

            Assert.That(initialTask.Result, Is.Not.Null, "Initial response should not be null");

            // Act
            // Second message references context from first
            var followUpTask = _controller.SendChatMessageAsync(TestChatFollowup);
            yield return followUpTask.WaitForTask();

            // Assert
            Assert.That(followUpTask.Result, Is.Not.Null, "Follow-up response should not be null");
            Assert.That(followUpTask.Result.Length, Is.GreaterThan(0), "Follow-up response should not be empty");
            Assert.That(followUpTask.Result, Does.Contain("8")
                    .Or.Contains("eight")
                    .Or.Contains("Eight"),
                "Response should contain the number 8 or equivalent");
        }

        [UnityTest]
        public IEnumerator SendChatMessage_AfterClear_ShouldNotMaintainContext()
        {
            // Arrange
            var task = _controller.SendChatMessageAsync(TestChatInitial);
            yield return task.WaitForTask();
            Assert.That(task.Result, Is.Not.Null, "Initial message response should not be null");

            // Act
            yield return _controller.ClearChatHistoryAsync().WaitForTask();
            task = _controller.SendChatMessageAsync(TestChatFollowup); // It has context
            yield return task.WaitForTask();

            // Assert
            Assert.That(task.Result, Is.Not.Null, "Response should not be null");
            Assert.That(task.Result, Does.Contain("context")
                    .Or.Contains("clarify")
                    .Or.Contains("specify")
                    .Or.Contains("do not know")
                    .Or.Contains("don't know")
                    .Or.Contains("have no access")
                    .Or.Contains("haven't access")
                    .Or.Contains("do not have access")
                    .Or.Contains("don't have access")
                    .Or.Contains("cannot answer")
                    .Or.Contains("can't answer")
                    .Or.Contains("cannot respond"),
                "Response should indicate lack of context");
            Assert.That(task.Result, Does.Not.Contain("8")
                    .Or.Not.Contains("eight")
                    .Or.Not.Contains("Eight"),
                "Response should not contain the number 8 or equivalent");
        }
    }
}