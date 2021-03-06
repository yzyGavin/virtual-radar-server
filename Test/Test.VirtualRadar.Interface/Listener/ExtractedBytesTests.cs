﻿// Copyright © 2012 onwards, Andrew Whewell
// All rights reserved.
//
// Redistribution and use of this software in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of the program's contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHORS OF THE SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualRadar.Interface.Listener;
using Test.Framework;

namespace Test.VirtualRadar.Interface.Listener
{
    [TestClass]
    public class ExtractedBytesTests
    {
        [TestMethod]
        public void ExtractedBytes_Constructor_Initialises_To_Known_State_And_Properties_Work()
        {
            var extractedBytes = new ExtractedBytes();

            TestUtilities.TestProperty(extractedBytes, r => r.Bytes, null, new byte[] {});
            TestUtilities.TestProperty(extractedBytes, r => r.ChecksumFailed, false);
            TestUtilities.TestProperty(extractedBytes, r => r.Format, ExtractedBytesFormat.None, ExtractedBytesFormat.ModeS);
            TestUtilities.TestProperty(extractedBytes, r => r.HasParity, false);
            TestUtilities.TestProperty(extractedBytes, r => r.Length, 0, 1);
            TestUtilities.TestProperty(extractedBytes, r => r.Offset, 0, 1);
        }

        [TestMethod]
        public void ExtractedBytes_Clone_Returns_A_Deep_Copy()
        {
            var originalByteArray = new byte[] {0x01, 0x02};

            foreach(var property in typeof(ExtractedBytes).GetProperties()) {
                var original = new ExtractedBytes();
                switch(property.Name) {
                    case "Bytes":           original.Bytes = originalByteArray; break;
                    case "ChecksumFailed":  original.ChecksumFailed = true; break;
                    case "Format":          original.Format = ExtractedBytesFormat.Port30003; break;
                    case "HasParity":       original.HasParity = true; break;
                    case "Length":          original.Length = 1; break;
                    case "Offset":          original.Offset = 1; break;
                    default:                throw new NotImplementedException();
                }

                var clone = (ExtractedBytes)original.Clone();
                Assert.AreNotSame(original, clone);

                switch(property.Name) {
                    case "Bytes":
                        Assert.IsNotNull(clone.Bytes);
                        Assert.AreNotSame(original.Bytes, clone.Bytes);
                        Assert.AreEqual(2, clone.Bytes.Length);
                        Assert.AreEqual(1, clone.Bytes[0]);
                        Assert.AreEqual(2, clone.Bytes[1]);
                        break;
                    case "ChecksumFailed":  Assert.AreEqual(true, clone.ChecksumFailed); break;
                    case "Format":          Assert.AreEqual(ExtractedBytesFormat.Port30003, clone.Format); break;
                    case "HasParity":       Assert.AreEqual(true, clone.HasParity); break;
                    case "Length":          Assert.AreEqual(1, clone.Length); break;
                    case "Offset":          Assert.AreEqual(1, clone.Offset); break;
                    default:                throw new NotImplementedException();
                }
            }
        }

        [TestMethod]
        public void ExtractedBytes_Equals_Returns_Correct_Value()
        {
            var obj1 = new ExtractedBytes() { Bytes = new byte[] { 0x01, 0x02, 0x03 }, Format = ExtractedBytesFormat.ModeS, Offset = 0, Length = 1, ChecksumFailed = false, HasParity = false };
            var obj2 = new ExtractedBytes() { Bytes = new byte[] { 0x01, 0x02, 0x03 }, Format = ExtractedBytesFormat.ModeS, Offset = 0, Length = 1, ChecksumFailed = false, HasParity = false };

            Assert.AreEqual(obj1, obj2);

            obj1.Bytes[2] = 0x0f;
            Assert.AreNotEqual(obj1, obj2);

            obj1.Bytes = obj2.Bytes;
            obj1.Format = ExtractedBytesFormat.Port30003;
            Assert.AreNotEqual(obj1, obj2);

            obj1.Format = obj2.Format;
            obj1.Offset = 1;
            Assert.AreNotEqual(obj1, obj2);

            obj1.Offset = obj2.Offset;
            obj1.Length = 2;
            Assert.AreNotEqual(obj1, obj2);

            obj1.Length = obj2.Length;
            obj1.ChecksumFailed = true;
            Assert.AreNotEqual(obj1, obj2);

            obj1.ChecksumFailed = obj2.ChecksumFailed;
            obj1.HasParity = true;
            Assert.AreNotEqual(obj1, obj2);
        }
    }
}
