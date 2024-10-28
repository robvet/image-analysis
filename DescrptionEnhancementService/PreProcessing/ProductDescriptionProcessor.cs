using DescrptionEnhancementService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DescrptionEnhancementService
{
    internal class ProductDescriptionProcessor : IProductDescriptionProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductDescriptionProcessor> _logger;

        public ProductDescriptionProcessor(IConfiguration configuration,
                                           ILogger<ProductDescriptionProcessor> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // Product Categories - along with keywords to categorize a product based on its description

        // The keywords are based on common terms used in product descriptions
        // The keywords are not exhaustive and may need to be updated based on the product catalog

        // The keywords are case-insensitive
        // The first category that matches a keyword is selected
        // If no category matches, the product is assigned to the "General" category

        private static readonly Dictionary<string, List<string>> CategoryKeywords = new Dictionary<string, List<string>>
        {
            { "Hot Food", new List<string> { "hot", "pizza", "sandwich", "burrito", "wrap" } },
            { "Beverages", new List<string> { "drink", "soda", "water", "coffee", "juice" } },
            { "Batteries", new List<string> { "battery", "AAA", "AA", "C", "D", "lithium" } },
            { "Candy Bars", new List<string> { "chocolate", "candy", "bar", "snack", "sweet" } },
            { "Cupcakes", new List<string> { "cupcake", "cake", "pastry", "baked", "dessert" } },
            { "General", new List<string>() } // Empty list for the General category as a fallback
        };

        // Define abbreviations and details for each category
        private static readonly Dictionary<string, Dictionary<string, string>> Abbreviations = new Dictionary<string, Dictionary<string, string>>
        {
            { "Hot Food", new Dictionary<string, string> {
                { "oz", "ounces" },
                { "lb", "pound" },
                { "cal", "calories" },
                { "mg", "milligrams" }
            }},
            { "Beverages", new Dictionary<string, string> {
                { "oz", "ounces" },
                { "ml", "milliliters" },
                { "mg", "milligrams" },
                { "caff", "caffeine" }
            }},
            { "Batteries", new Dictionary<string, string> {
                { "mAh", "milliampere-hour" },
                { "V", "volt" },
                { "recharge", "rechargeable" }
            }},
            { "Candy Bars", new Dictionary<string, string> {
                { "g", "grams" },
                { "cal", "calories" },
                { "mg", "milligrams" },
                { "sugar", "sugar content" }
            }},
            { "General", new Dictionary<string, string> {
                { "oz", "ounces" },
                { "lb", "pound" },
                { "g", "grams" },
                { "mg", "milligrams" }
            }}
        };


        // Method to identify the product category based on keywords
        public async Task<string> IdentifyCategory(string description)
        {
            foreach (var category in CategoryKeywords)
            {
                if (category.Value.Any(keyword => description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    return category.Key;
                }
            }
            return "General"; // Default if no category matches
        }

        // Method to generate a prompt based on the identified category
        public async Task<string> GeneratePromptAsync(string description)
        {
            string category = await IdentifyCategory(description);
            var abbreviations = Abbreviations.ContainsKey(category) ? Abbreviations[category] : new Dictionary<string, string>();

            // Build abbreviation list for the prompt
            StringBuilder abbreviationsText = new StringBuilder();
            foreach (var abbr in abbreviations)
            {
                abbreviationsText.AppendLine($"- {abbr.Key}: {abbr.Value}");
            }

            // Create prompt text
            StringBuilder prompt = new StringBuilder();
            prompt.AppendLine($"Product Type: {category}\n");
            prompt.AppendLine("Common Abbreviations:");
            prompt.AppendLine(abbreviationsText.ToString());
            prompt.AppendLine("Convert the following cryptic product description from a vendor into a clear, customer-friendly description. Follow these rules:");
            prompt.AppendLine("1. Expand abbreviations based on the provided list.");
            prompt.AppendLine("2. Replace technical terms with simple, conversational language.");
            prompt.AppendLine("3. Highlight key features and benefits of the product.");
            prompt.AppendLine("\nExamples:\n");
            prompt.AppendLine("Example 1\nInput: \"Choc bar, 200 cal, 5g sugar, 2g protein.\"");
            prompt.AppendLine("Output: \"Chocolate bar with 200 calories, containing 5 grams of sugar and 2 grams of protein. A sweet treat to satisfy cravings.\"\n");
            prompt.AppendLine("Example 2\nInput: \"AAA batt, 1200 mAh, long-life, 1.5V.\"");
            prompt.AppendLine("Output: \"AAA battery with a 1200 milliampere-hour capacity, providing long-lasting power at 1.5 volts.\"\n");
            prompt.AppendLine($"Now, transform this product description accordingly:\n\nInput: \"{description}\"\nOutput:");

            return prompt.ToString();
        }
    }
}
