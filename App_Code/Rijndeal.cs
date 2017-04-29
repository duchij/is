///////////////////////////////////////////////////////////////////////////////
// SAMPLE: Symmetric key encryption and decryption using Rijndael algorithm.
// 
// To run this sample, create a new Visual C# project using the Console
// Application template and replace the contents of the Class1.cs file with
// the code below.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// 
// Copyright (C) 2002-2013 Obviex(TM). All rights reserved.
// 
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

/// <summary>
/// This class uses a symmetric key algorithm (Rijndael/AES) to encrypt and 
/// decrypt data. As long as encryption and decryption routines use the same
/// parameters to generate the keys, the keys are guaranteed to be the same.
/// The class uses static functions with duplicate code to make it easier to
/// demonstrate encryption and decryption logic. In a real-life application, 
/// this may not be the most efficient way of handling encryption, so - as
/// soon as you feel comfortable with it - you may want to redesign this class.
/// </summary>
public class Rijndael
{

    /// <summary>
    /// Decrypts a base64 AES/Rijndeal encoded string with vector (16 bytes)
    /// </summary>
    /// <param name="text">Base 64bit string  to decode</param>
    /// <param name="sid">Vector</param>
    /// <returns>SortedList status/result</returns>

    public static SortedList decryptJsAes(string text, string sid)
    {
        SortedList result = new SortedList();

        using (var crypto = new RijndaelManaged())
        {
            System.Text.UTF8Encoding txtenc = new System.Text.UTF8Encoding();

            byte[] textBytes = Convert.FromBase64String(text);

            string kP = sid.Substring(0, 16);
            byte[] vector = txtenc.GetBytes(kP);

            try
            {
                crypto.Mode = CipherMode.CBC;
                crypto.Padding = PaddingMode.PKCS7;
                crypto.BlockSize = 128;
                crypto.KeySize = 128;
                crypto.FeedbackSize = 128;

                crypto.IV = vector;

                crypto.Key = vector;

                ICryptoTransform decryptor = crypto.CreateDecryptor(crypto.Key, crypto.IV);

                MemoryStream ms = new MemoryStream(textBytes);
                CryptoStream cr = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                byte[] output = new byte[textBytes.Length];
                int readBytes = cr.Read(output, 0, textBytes.Length);
                
                result["result"] = Encoding.UTF8.GetString(output).Replace("\0","");
                result["status"] = true;

            }
            catch (Exception ex)
            {
                result["status"] = false;
                result["result"] = ex.ToString();
            }
            // this.info_txt.Text = plainText;
        }


        return result;
    }
    /// <summary>
    /// Encrypts plain Text with AES/Rijndeal with vector
    /// </summary>
    /// <param name="plaintext">Normal plain text</param>
    /// <param name="sid">Vector to encrypt uses 16bytes</param>
    /// <returns>SortedList status/result</returns>
    public static SortedList encryptAesJs(string plaintext, string sid)
    {
             


        SortedList result = new SortedList();

        byte[] textBytes = Encoding.UTF8.GetBytes(plaintext);

        string kP = sid.Substring(0, 16);
        byte[] vector = Encoding.UTF8.GetBytes(kP);

        using (var crypto = new RijndaelManaged())
        {
            crypto.Mode = CipherMode.CBC;
            crypto.Padding = PaddingMode.PKCS7;
            crypto.BlockSize = 128;
            crypto.KeySize = 128;
            crypto.FeedbackSize = 128;

            crypto.IV = vector;

            crypto.Key = vector;


            ICryptoTransform encryptor = crypto.CreateEncryptor(crypto.Key, crypto.IV);

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cr = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sEncrypt = new StreamWriter(cr))
                        {
                            sEncrypt.Write(plaintext);
                        }

                        //string tmp = Convert.ToBase64String(cBytes);
                        byte[] encr = ms.ToArray();
                        result["result"] = Convert.ToBase64String(encr);
                        result["status"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result["status"] = false;
                result["result"] = ex.ToString();
            }
            // this.info_txt.Text = plainText;
        }

        return result;


    }





}
//
// END OF FILE
///////////////////////////////////////////////////////////////////////////////