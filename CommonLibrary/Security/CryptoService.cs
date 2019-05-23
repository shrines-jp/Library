using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CommonLibrary.Security
{
    public class CryptoService
    {
        #region enums, constants & fields
        /// <summary>
        /// 비대칭 암/복호화 알고리즘 상수
        /// </summary>
        public enum CryptoTypes
        {
            encTypeDES = 0,
            encTypeRC2,
            encTypeRijndael,
            encTypeTripleDES
        }

        private const string CRYPT_DEFAULT_PASSWORD = "abcd!@#";
        private const CryptoTypes CRYPT_DEFAULT_METHOD = CryptoTypes.encTypeDES;

        private byte[] mKey = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private byte[] mIV = { 65, 110, 68, 26, 69, 178, 200, 219 };
        private byte[] SaltByteArray = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        private CryptoTypes mCryptoType = CRYPT_DEFAULT_METHOD;
        private string mPassword = CRYPT_DEFAULT_PASSWORD;

        private string sInputFile = string.Empty, sOutputFile = string.Empty;
        #endregion

        #region Constructors
        /// <summary>
        /// 디폴트 생성자

        /// </summary>
        public CryptoService()
        {
            calculateNewKeyAndIV();
        }

        public CryptoService(string mKey, string mIV)
        {
            this.mKey = Encoding.UTF8.GetBytes(mKey);
            this.mIV = Encoding.UTF8.GetBytes(mIV);
        }


        /// <summary>
        /// 생성자(암/복호화 타입을 지정)
        /// </summary>
        /// <param name="CryptoType"></param>
        public CryptoService(CryptoTypes CryptoType)
        {
            this.CryptoType = CryptoType;
        }
        #endregion

        #region Props
        /// <summary>
        ///     type of encryption / decryption used
        /// </summary>
        public CryptoTypes CryptoType
        {
            get
            {
                return mCryptoType;
            }
            set
            {
                mCryptoType = value;
                calculateNewKeyAndIV();
            }
        }

        /// <summary>
        ///     Passsword Key Property.
        ///     The password key used when encrypting / decrypting
        /// </summary>
        public string Password
        {
            get
            {
                return mPassword;
            }
            set
            {
                if (mPassword != value)
                {
                    mPassword = value;
                    calculateNewKeyAndIV();
                }
            }
        }
        #endregion

        #region Encryption
        /// <summary>
        /// 텍스트를 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <returns>암호화된 텍스트</returns>
        public string Encrypt(string inputText)
        {
            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //get byte representation of string
            byte[] inputBytes = UTF8Encoder.GetBytes(inputText);

            //convert back to a string
            return Convert.ToBase64String(EncryptDecrypt(inputBytes, true));

        }

        /// <summary>
        /// 사용자가 지정한 패스워드로 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <param name="password">암호화에 사용할 패스워드</param>
        /// <returns>암호화된 텍스트</returns>
        public string Encrypt(string inputText, string password)
        {
            this.Password = password;

            return this.Encrypt(inputText);
        }

        /// <summary>
        /// 사용자가 지정한 cryptoType과 패스워드로 텍스트를 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <param name="password">암호화에 사용할 패스워드</param>
        /// <param name="cryptoType">암호화 타입</param>
        /// <returns>암호화된 텍스트</returns>

        public string Encrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            mCryptoType = cryptoType;

            return this.Encrypt(inputText, password);
        }

        /// <summary>
        /// 사용자가 지정한 cryptoType으로 텍스트를 암호화한다.
        /// </summary>
        /// <param name="inputText">암호화할 텍스트</param>
        /// <param name="cryptoType">암호화 타입</param>
        /// <returns>암호화된 텍스트</returns>
        public string Encrypt(string inputText, CryptoTypes cryptoType)
        {
            this.CryptoType = cryptoType;

            return this.Encrypt(inputText);
        }



        #endregion

        #region Decryption

        /// <summary>
        /// 텍스트를 복호화한다.
        /// </summary>
        /// <param name="inputText">복호화할 텍스트</param>
        /// <returns>복호화된 텍스트</returns>
        public string Decrypt(string inputText)
        {
            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();

            //get byte representation of string
            byte[] inputBytes = Convert.FromBase64String(inputText);

            //convert back to a string
            return UTF8Encoder.GetString(EncryptDecrypt(inputBytes, false));
        }

        /// <summary>
        /// 사용자가 지정한 패스워드키에 의해 텍스트를 복호화한다.
        /// </summary>
        /// <param name="inputText">복호화할 텍스트</param>
        /// <param name="password">복호화할 때 사용할 패스워드</param>
        /// <returns>복호화된 텍스트</returns>
        public string Decrypt(string inputText, string password)
        {
            this.Password = password;

            return Decrypt(inputText);
        }

        /// <summary>
        /// 사용자가 지정한 cryptoType과 패스워드로 텍스트를 복호화한다.
        /// </summary>
        /// <param name="inputText">복호화할 텍스트</param>
        /// <param name="password">복호화에 사용할 패스워드</param>
        /// <param name="cryptoType">복호화 타입</param>
        /// <returns>복호화된 텍스트</returns>
        public string Decrypt(string inputText, string password, CryptoTypes cryptoType)
        {
            mCryptoType = cryptoType;

            return Decrypt(inputText, password);
        }

        /// <summary>
        /// 사용자가 지정한 cryptoType으로 텍스트를 복호화한다.
        /// </summary>
        /// <param name="inputText">복호화할 텍스트</param>
        /// <param name="cryptoType">복호화 타입</param>
        /// <returns>복호화된 텍스트</returns>
        public string Decrypt(string inputText, CryptoTypes cryptoType)
        {
            this.CryptoType = cryptoType;

            return Decrypt(inputText);
        }
        #endregion

        #region Symmetric Engine
        /// <summary>
        ///     performs the actual enc/dec.
        /// </summary>
        /// <param name="inputBytes">input byte array</param>
        /// <param name="Encrpyt">wheather or not to perform enc/dec</param>
        /// <returns>byte array output</returns>
        private byte[] EncryptDecrypt(byte[] inputBytes, bool Encrpyt)
        {
            //get the correct transform
            ICryptoTransform transform = getCryptoTransform(Encrpyt);

            //memory stream for output
            MemoryStream memStream = new MemoryStream();

            try
            {
                //setup the cryption - output written to memstream
                CryptoStream cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);

                //write data to cryption engine
                cryptStream.Write(inputBytes, 0, inputBytes.Length);

                //we are finished
                cryptStream.FlushFinalBlock();

                //get result
                byte[] output = memStream.ToArray();

                //finished with engine, so close the stream
                cryptStream.Close();

                return output;
            }
            catch (Exception e)
            {
                //throw an error
                throw new Exception("Error in symmetric engine. Error : " + e.Message, e);
            }
        }

        /// <summary>
        ///     returns the symmetric engine and creates the encyptor/decryptor
        /// </summary>
        /// <param name="encrypt">whether to return a encrpytor or decryptor</param>
        /// <returns>ICryptoTransform</returns>
        private ICryptoTransform getCryptoTransform(bool encrypt)
        {
            SymmetricAlgorithm SA = selectAlgorithm();

            SA.Key = mKey;
            SA.IV = mIV;
            if (encrypt)
            {
                return SA.CreateEncryptor();
            }
            else
            {
                return SA.CreateDecryptor();
            }
        }

        /// <summary>
        ///     returns the specific symmetric algorithm acc. to the cryptotype
        /// </summary>
        /// <returns>SymmetricAlgorithm</returns>
        private SymmetricAlgorithm selectAlgorithm()
        {
            SymmetricAlgorithm SA;

            switch (mCryptoType)
            {
                case CryptoTypes.encTypeDES:
                    SA = DES.Create();
                    break;

                case CryptoTypes.encTypeRC2:
                    SA = RC2.Create();
                    break;

                case CryptoTypes.encTypeRijndael:
                    SA = Rijndael.Create();
                    break;

                case CryptoTypes.encTypeTripleDES:
                    SA = TripleDES.Create();
                    break;

                default:
                    SA = TripleDES.Create();
                    break;
            }

            return SA;
        }

        /// <summary>
        ///     calculates the key and IV acc. to the symmetric method from the password
        ///     key and IV size dependant on symmetric method
        /// </summary>
        private void calculateNewKeyAndIV()
        {
            //use salt so that key cannot be found with dictionary attack
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(mPassword, SaltByteArray);

            SymmetricAlgorithm algo = selectAlgorithm();

            mKey = pdb.GetBytes(algo.KeySize / 8);
            mIV = pdb.GetBytes(algo.BlockSize / 8);
        }
        #endregion

        #region File Encryption/Decryption
        /// <summary>
        /// 암호화 처리 함수(DES 알고리즘)
        /// </summary>
        /// <param name="sInputFile">입력 파일</param>
        /// <param name="sOutputFile">출력 파일</param>
        public void EncryptFile(string sInputFile, string sOutputFile)
        {
            EncryptOrDecryptFile(sInputFile, sOutputFile, 1);
        }

        /// <summary>
        /// 복호화 처리 함수(DES 알고리즘)
        /// </summary>
        /// <param name="sInputFile">입력 파일</param>
        /// <param name="sOutputFile">출력 파일</param>
        public void DecryptFile(string sInputFile, string sOutputFile)
        {
            EncryptOrDecryptFile(sInputFile, sOutputFile, 2);
        }

        /// <summary>
        /// 암호화/복호화 처리 함수(DES 알고리즘)
        /// </summary>
        /// <param name="sInputFile">입력 파일</param>
        /// <param name="sOutputFile">출력 파일</param>
        /// <param name="Direction">암호화(1)/복호화(2) 여부</param>
        private void EncryptOrDecryptFile(string sInputFile, string sOutputFile, int Direction)
        {
            // 파일 스트림을 만들어 입력 및 출력 파일을 처리합니다.
            FileStream fsInput = new FileStream(sInputFile, FileMode.Open, FileAccess.Read);
            FileStream fsOutput = new FileStream(sOutputFile, FileMode.OpenOrCreate, FileAccess.Write);
            fsOutput.SetLength(0);

            // 암호화/암호 해독 프로세스 중 필요한 변수입니다.
            byte[] byteBuffer = new byte[4096]; // 처리를 위해 바이트 블록을 보유합니다.

            long nBytesProcessed = 0; // 암호화된 바이트의 실행 카운트

            long nFileLength = fsInput.Length;
            int iBytesInCurrentBlock;

            CryptoStream csMyCryptoStream = null;

            switch (mCryptoType)
            {
                case CryptoTypes.encTypeDES:
                    csMyCryptoStream = DESCSP(fsOutput, Direction);
                    break;

                case CryptoTypes.encTypeRC2:
                    csMyCryptoStream = RC2CSP(fsOutput, Direction);
                    break;

                case CryptoTypes.encTypeRijndael:
                    csMyCryptoStream = RijndaelCSP(fsOutput, Direction);
                    break;

                case CryptoTypes.encTypeTripleDES:
                    csMyCryptoStream = TripleDESCSP(fsOutput, Direction);
                    break;

                default:
                    csMyCryptoStream = DESCSP(fsOutput, Direction);
                    break;
            }

            // 입력 파일에서 읽은 다음 암호화하거나 암호를 해독하고
            // 출력 파일에 씁니다.
            while (nBytesProcessed < nFileLength)
            {
                iBytesInCurrentBlock = fsInput.Read(byteBuffer, 0, 4096);
                csMyCryptoStream.Write(byteBuffer, 0, iBytesInCurrentBlock);
                nBytesProcessed = nBytesProcessed + (long)iBytesInCurrentBlock;
            }

            csMyCryptoStream.Close();
            fsInput.Close();
            fsOutput.Close();
        }

        private CryptoStream DESCSP(FileStream fsOutput, int Direction)
        {
            DESCryptoServiceProvider Provider = new DESCryptoServiceProvider();

            CryptoStream csMyCryptoStream = null;

            // 암호화나 암호 해독을 위한 설정
            switch (Direction)
            {
                case 1: // 암호화

                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;

                case 2:// 복호화

                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }

            return csMyCryptoStream;
        }

        private CryptoStream RC2CSP(FileStream fsOutput, int Direction)
        {
            RC2CryptoServiceProvider Provider = new RC2CryptoServiceProvider();

            CryptoStream csMyCryptoStream = null;

            // 암호화나 암호 해독을 위한 설정
            switch (Direction)
            {
                case 1: // 암호화

                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;

                case 2:// 복호화

                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }

            return csMyCryptoStream;
        }

        private CryptoStream RijndaelCSP(FileStream fsOutput, int Direction)
        {
            RijndaelManaged Provider = new RijndaelManaged();

            CryptoStream csMyCryptoStream = null;

            // 암호화나 암호 해독을 위한 설정
            switch (Direction)
            {
                case 1: // 암호화

                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;

                case 2:// 복호화

                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }

            return csMyCryptoStream;
        }

        private CryptoStream TripleDESCSP(FileStream fsOutput, int Direction)
        {
            TripleDESCryptoServiceProvider Provider = new TripleDESCryptoServiceProvider();

            CryptoStream csMyCryptoStream = null;

            // 암호화나 암호 해독을 위한 설정
            switch (Direction)
            {
                case 1: // 암호화

                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateEncryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;

                case 2:// 복호화

                    csMyCryptoStream = new CryptoStream(fsOutput, Provider.CreateDecryptor(mKey, mIV), CryptoStreamMode.Write);
                    break;
            }

            return csMyCryptoStream;

        }

        private byte[] GetKeyByteArray(string sPassword)
        {
            byte[] byteTemp = new byte[7];

            sPassword = sPassword.PadRight(8);

            byteTemp = System.Text.Encoding.ASCII.GetBytes(sPassword.ToCharArray());

            return byteTemp;
        }

        #endregion

    }
}
