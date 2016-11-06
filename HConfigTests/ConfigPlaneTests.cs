using System;
using System.Collections.Generic;
using HConfig;
using NUnit.Framework;



namespace HConfigTests
{
    [TestFixture]
    public class ConfigPlaneTests
    {
        [Test]
        public void ChangingLevelNameAfterSetting_CausesException()
        {
           throw new NotImplementedException();
        }
        [Test]
        public void ChangingLevelValue_ValueReturnedIsEmptyString()
        {

            throw new NotImplementedException();
        }

        [Test]
        public void AddingNewSpoke_NewSpokeStored()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void AddingExistingSpoke_NewSpokeSaved()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GettingSpoke_ReturnsSpokeIfPresent()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GettingSpoke_ReturnsNullIfNotPresent()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetSpoke_ReturnsTrueIfPresent()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetSpoke_OutputsValueIfPresent()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetSpoke_ReturnsFalseIfNotPresent()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetSpoke_OutputsNullIfNotPresent()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetValue_UsesCorrectSpokeForValue()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetValue_UsingContextUsesCorrectSpokeForValue()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetValue_FallsBackToHubIfNoValueOnSpoke()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetValue_UsingContextFallsBackToHubIfNoValueOnSpoke()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GetValue_UsesCorrectSpokeForValue()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GetValue_FallsBackToHubIfNoValueOnSpoke()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GetValue_UsingContextUsesCorrectSpokeForValue()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GetValue_UsingContextFallsBackToHubIfNoValueOnSpoke()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetValue_UsingContextThrowsExceptionIfNoContextSet()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetValue_UsingContextThrowsExceptionIfNoContextSet()
        {
            throw new NotImplementedException();
        }



        [Test]
        public void UpsertingNewNameAndValue_CausesValueToBeStored()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void UpsertingExistingNameAndValue_CausesValueToBeStored()
        {
            throw new NotImplementedException();

        }

        [Test]
        public void TryGetValue_ReturnsFalseIfNoFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TryGetValue_ReturnsTrueIfFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TryGetValue_SetsValueIfFound()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetValue_ReturnsCorrectValueWhenMultiopleEntries()
        {
            throw new NotImplementedException();
        }


        [Test]
        public void GetValue_ReturnsNullIfNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetValue_ReturnsValueIfFound()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GetValue_ReturnsCorrectValueWhenMultiopleEntries()
        {
            throw new NotImplementedException();
        }
    }
}
