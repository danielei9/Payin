using System;

namespace Xp.Application.Tsm.GlobalPlatform.Utilities
{
	public class RandomGenerator
	{
		/// <summary>
		/// for testing purpose
		/// </summary>
		private static byte[] RandomSequence;

		/// <summary>
		/// This function is only available during test to set next random, and thus have tests which can be repeated.
		/// </summary>
		/// <param name="randomSequence">randomSequence the next random sequence</param>
		public static void setRandomSequence(byte[] randomSequence)
		{
			//logger.warn("setting a fixed value for random (this feature is only used for testing purpose)");
			RandomGenerator.RandomSequence = randomSequence;
		}
		/// <summary>
		/// Generate a random byte array with a specific size.
		/// </summary>
		/// <param name="size">the size of the random byte array</param>
		/// <returns>the random byte array</returns>
		public static byte[] generateRandom(int size)
		{
			// *************** FOR TEST **********************
			if (RandomSequence != null)
			{
				if (RandomSequence.Length != size)
					throw new ArgumentException("Size is different from random sequence length");
				return RandomSequence.CloneArray();
			}

			// classical random
			/*SecureRandom secureRandom;
			try
			{
				secureRandom = SecureRandom.getInstance("SHA1PRNG");
			}
			catch (NoSuchAlgorithmException ex)
			{
				throw new UnsupportedOperationException("Cannot create random sequence: " + ex.getMessage());
			}

			return secureRandom.generateSeed(size);*/
			return new byte[size];
		}
	}
}
