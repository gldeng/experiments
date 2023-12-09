using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Experiment;

public class TestCase
{
    [JsonPropertyName("salt")] public string Salt { get; set; }

    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("id_hash")] public string IdHash { get; set; }
}

public class UnitTest1
{
    private static readonly string TestData = """
                                              [
                                                  {
                                                    "salt": "8a7e44fa4a244e28a65ed89962997c41",
                                                    "id": "01",
                                                    "id_hash": "ac379499210dc4af65b537bd5deed7033d664cb2b55965105e8ad68fadb13456"
                                                  },
                                                  {
                                                    "salt": "4d9860e98de047db9276ff931604cd7c",
                                                    "id": "01010101010101010101",
                                                    "id_hash": "50f26c542a3a50493f1472955fc5cf9c16a867bc4ab6afc1d03b75846371321f"
                                                  },
                                                  {
                                                    "salt": "8d1fca931514443ea5072cc8ee8faf2d",
                                                    "id": "4233507b287e2b835780",
                                                    "id_hash": "87912cfddc3353ec8536a20e755ae450e7215a925c81c8891dede7901a32a7ed"
                                                  },
                                                  {
                                                    "salt": "77dce9fe8d84429096e6f1f172c2b97c",
                                                    "id": "7fd5533ca87a57ac0da5fd604b841fe06dd929540c44f5f47368169cce6b5dc502e18fec90be231d3e02f7b18ad2da97dbfca12818ec6e7c29930d8fa5975dbeb2f0c6fd15f13823b0bbe11798fa2e4096b3c6032d4eedc2aa8dc49d239d6995fa9d771a",
                                                    "id_hash": "2ead0814fbfa6721ef056f3fd96d52163538673acf27a65054b3b09b34b97f2c"
                                                  },
                                                  {
                                                    "salt": "05805842933f45ef9e804c2a95a976fc",
                                                    "id": "aad5c0309b1ad8c563ef68de647fa72e6be6b88c526ad68591ade1c44dfabc87b69bc20bb89119802f1fcf12312f1013860b373a6859d30a5b5b16232adbb7f6e2d64938fa6672312d25397e9dbcc33d9951091cde0d4028b561ad98546d7222ae41fe3176a156d3668477c058e41b49ecc6d79681e7185f81ee081a24d084dbde0b53d0494717c88b7b3b76e07835f2682cdc4e1a535df73ced86afbfd82242dd773dade2ef9ec67c3716bc0af4085346423e02364723480f45a17c6b7db1316828a89783f1bbd9",
                                                    "id_hash": "c795eb18ab37cc62610a1ef19a7baaad66434d8ff4683dce32f77e955f7aa58e"
                                                  },
                                                  {
                                                    "salt": "0cfea7956cc74c40a70dc1199e0a7993",
                                                    "id": "01010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101",
                                                    "id_hash": "c2c0537318f399bb32f4707fe2b236683a5f406a87c4353165012b1bdad470e4"
                                                  },
                                                  {
                                                    "salt": "777340696bde4886a9c757f647c627e0",
                                                    "id": "010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101",
                                                    "id_hash": "aeb59c96587c606bd29f628b4551c5d2dd50fb38bdda3aec24aabab7a156a322"
                                                  },
                                                  {
                                                    "salt": "ae6dc66bc98b48b5b1ba3c7dd5c015ed",
                                                    "id": "abababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababab",
                                                    "id_hash": "ea6c869ff338f90b08c60d34b877b1177b8d20f34b30d594e602265c63fe7d69"
                                                  },
                                                  {
                                                    "salt": "daf184c7fd6442d3bcfa76fea08c533c",
                                                    "id": "ababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababab",
                                                    "id_hash": "2cb1dee4f430856a38e521f423c2aed9e02260cce87efb596458a9009a16388d"
                                                  }
                                                ]
                                              """;

    public static byte[] PadByteArrayToLength(byte[] originalArray, int targetLength)
    {
        byte[] paddedArray = new byte[targetLength];
        Array.Copy(originalArray, paddedArray, Math.Min(originalArray.Length, targetLength));

        return paddedArray;
    }

    public byte[] CalculateIdentifierHash(byte[] identifier, byte[] salt)
    {
        var hash1 = Sha256(identifier);
        var hash2 = Sha256(hash1.Concat(salt).ToArray());
        return hash2;
    }

    [Fact]
    public void Test1()
    {
        var testCases = JsonSerializer.Deserialize<List<TestCase>>(TestData);
        foreach (var testCase in testCases)
        {
            var idHash =
                CalculateIdentifierHash(HexStringToByteArray(testCase.Id), HexStringToByteArray(testCase.Salt));
            Assert.Equal(idHash, HexStringToByteArray(testCase.IdHash));
        }
    }

    private static byte[] Sha256(byte[] bytes)
    {
        using var shA256 = SHA256.Create();
        return shA256.ComputeHash(bytes);
    }

    static byte[] HexStringToByteArray(string hexString)
    {
        int length = hexString.Length;
        byte[] byteArray = new byte[length / 2];

        for (int i = 0; i < length; i += 2)
        {
            byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }

        return byteArray;
    }
}