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
                                                        "salt": "6489b951119243958d720c41a810f448",
                                                        "id": "01",
                                                        "id_hash": "a7db603681b8bde93a66659828384aa8cb52153b019bfea38787b71528effbfa"
                                                      },
                                                      {
                                                        "salt": "6b8bd0f79eed4f59b726e73f6347aedf",
                                                        "id": "01010101010101010101",
                                                        "id_hash": "360ee45c0542ce37ce41ebb80d9d37da73c5809eff776ffc9b405e1100b5faaa"
                                                      },
                                                      {
                                                        "salt": "40a9189ceb144077a13212f32c735cdd",
                                                        "id": "57708cf9161c05230812",
                                                        "id_hash": "63cd07d664789477c019a4a2d250ffb6369438ff12306d9d0b56cccc01c1d85b"
                                                      },
                                                      {
                                                        "salt": "8567e9fc305c45f4a229aa14040e9307",
                                                        "id": "555778ee2330ef2440d2a427b1aa2b6176824dd08901626372f7cb1b0eb2d2957a41bfee5225fb26b7769ba5d2eaef2e0a152b03bece584887e83b55706cf1f93fa2f08f15d7ced816d16479993ecd32f1108f4198deaa1f881ba127e8360bcda34ebab3",
                                                        "id_hash": "303bd060cd9ba4d81e4f4bf18298e2b6fa7a619a7288ff92a8ef6bab137ab237"
                                                      },
                                                      {
                                                        "salt": "06814ece436f4d718421f07e903b39c9",
                                                        "id": "271ca8adb8fd2c21984a6a01edd86a7c3e4db6c38b211271484d604418007e263972eba25cd434e2bea579d372909bf6f907358b848b06bf02bfef048a6ee257d31da4768db622b7883e11330bcbc4107a39f21f052cbb177d2baccea78a22364997b774d4ba44c414d3c15fc142ac2fd69ae64c02cd96ac2af8e2bb4903d1d582f825dd2ff2a9e6cfc592532e38e9987059c0714cc58f21340366f77085946875a2da8c5b05c0d609f67d12ed05139e1338b8a46b0e98bdc3cbbcaadd16e98cffe55825dd05ac86",
                                                        "id_hash": "6d18fe337b862f9c4b63f71c09c28bc857d615a025f8588fac5ba31ac9851f5b"
                                                      },
                                                      {
                                                        "salt": "81d96d0c1f9f4ea1a9965f4a627d1750",
                                                        "id": "01010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101",
                                                        "id_hash": "6660692fd00d8a99380472ca0398d8190b4cddc2a242ef5fe02f70f890f925b4"
                                                      },
                                                      {
                                                        "salt": "ebe24272bdab44ff9120bb954a437d84",
                                                        "id": "010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101",
                                                        "id_hash": "02bb2784b3b7c526529ed77d80f02f888cd4d079c07ca541e11321a6d7114ed3"
                                                      },
                                                      {
                                                        "salt": "653b9686fee8416081409bde09b9567b",
                                                        "id": "abababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababab",
                                                        "id_hash": "500272442e69408142c1c35a714adefc361634128429dd2e12a84694ebbd643a"
                                                      },
                                                      {
                                                        "salt": "8bd20240b5b0420a9b90d58c8d8b1688",
                                                        "id": "ababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababababab",
                                                        "id_hash": "6dcbff9e82d5a57ad2f76fd660d6eea6c68a9b0a48081448b1992536be8a2381"
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
        var hash1 = Sha256(PadByteArrayToLength(identifier, 256));
        var hash2 = Sha256(PadByteArrayToLength(salt, 16).Concat(hash1).ToArray());
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