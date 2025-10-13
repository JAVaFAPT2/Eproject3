namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.CreateVehicleModel
{
    public class CreateVehicleModelCommandHandler(IRepository<VehicleModel> modelRepository) : IRequestHandler<CreateVehicleModelCommand, string>
    {
        public async Task<string> Handle(CreateVehicleModelCommand request, CancellationToken cancellationToken)
        {
            // Check if model number already exists
            var existing = await modelRepository.FindAsync(vm => vm.ModelNumber == request.ModelNumber, cancellationToken);
            if (existing.Any())
            {
                throw new InvalidOperationException("Model number already exists");
            }

            // Enforce slug generation and uniqueness for level-2 models (variants)
            string? slugToUse = request.Slug;
            if (request.Level == 2)
            {
                slugToUse = GenerateSlug(string.IsNullOrWhiteSpace(request.Slug) ? request.Name : request.Slug);

                // ensure uniqueness
                var uniqueSlug = await EnsureUniqueSlug(slugToUse, cancellationToken);
                slugToUse = uniqueSlug;
            }

            var vehicleModel = new VehicleModel(
                request.ModelNumber,
                request.Name,
                request.Price,
                request.Description,
                request.ParentId,
                request.Level,
                slugToUse);

            await modelRepository.AddAsync(vehicleModel, cancellationToken);

            return vehicleModel.ModelNumber;
        }

        private static string GenerateSlug(string text)
        {
            // basic slugify: lower, trim, replace spaces with '-', remove invalid chars
            var normalized = text.ToLowerInvariant().Trim();
            var chars = normalized.Select(ch => char.IsLetterOrDigit(ch) ? ch : (ch == ' ' || ch == '-' ? '-' : '\0'))
                                  .Where(ch => ch != '\0')
                                  .ToArray();
            var slug = new string(chars);
            while (slug.Contains("--")) slug = slug.Replace("--", "-");
            return slug.Trim('-');
        }

        private async Task<string> EnsureUniqueSlug(string baseSlug, CancellationToken ct)
        {
            var slug = baseSlug;
            var suffix = 1;
            while (true)
            {
                var found = await modelRepository.FindAsync(m => m.Slug == slug, ct);
                if (!found.Any()) return slug;
                suffix++;
                slug = $"{baseSlug}-{suffix}";
            }
        }
    }
}
