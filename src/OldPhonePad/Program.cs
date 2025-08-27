namespace OldPhonePad
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("OldPhonePad (.NET 8)");
            Console.WriteLine("Type a sequence and ends with # (example: 4433555 555666#):");

            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Empty input.");
                return;
            }

            try
            {
                string output = OldPhonePadConverter.OldPhonePad(input);
                Console.WriteLine($"Result: {output}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
