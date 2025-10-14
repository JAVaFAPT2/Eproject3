using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Migrations.MigrateVehicleModelsV2
{
    /// <summary>
    /// Idempotent migration handler. Assumes existing VehicleModel documents may have null ParentId/Level/Slug.
    /// Migration rules:
    /// - If Level is 0 or <1, set Level=1 by default (root category) and ParentId=null
    /// - If ParentId is not null, ensure Level=2
    /// - For Level=2, generate Slug if missing using lowercase name and ensure uniqueness
    /// - No destructive changes; VehiclePhoto already references ModelId, VehicleSpec references ModelId
    /// </summary>
    public class MigrateVehicleModelsV2CommandHandler(IRepository<VehicleModel> modelRepository) : IRequestHandler<MigrateVehicleModelsV2Command, MigrateVehicleModelsV2Result>
    {
        public async Task<MigrateVehicleModelsV2Result> Handle(MigrateVehicleModelsV2Command request, CancellationToken cancellationToken)
        {
            var result = new MigrateVehicleModelsV2Result();
            var allModels = await modelRepository.GetAllAsync(cancellationToken);

            // Build existing slug set for uniqueness
            var existingSlugs = new HashSet<string>(allModels.Where(m => !string.IsNullOrWhiteSpace(m.Slug)).Select(m => m.Slug!), StringComparer.OrdinalIgnoreCase);

            foreach (var m in allModels)
            {
                var updated = false;

                if (m.Level < 1)
                {
                    m.SetHierarchy(null, 1);
                    result.Level1Assigned++;
                    updated = true;
                }

                if (!string.IsNullOrWhiteSpace(m.ParentId) && m.Level != 2)
                {
                    m.SetHierarchy(m.ParentId, 2);
                    result.Level2Assigned++;
                    updated = true;
                }

                if (m.Level == 2 && string.IsNullOrWhiteSpace(m.Slug))
                {
                    var baseSlug = GenerateSlug(m.Name);
                    var unique = EnsureUniqueSlug(baseSlug, existingSlugs);
                    m.SetSlug(unique);
                    existingSlugs.Add(unique);
                    result.SlugsGenerated++;
                    updated = true;
                }

                if (updated)
                {
                    await modelRepository.UpdateAsync(m, cancellationToken);
                    result.ModelsUpdated++;
                }
            }

            return result;
        }

        private static string GenerateSlug(string text)
        {
            var normalized = text.ToLowerInvariant().Trim();
            var chars = normalized.Select(ch => char.IsLetterOrDigit(ch) ? ch : (ch == ' ' || ch == '-' ? '-' : '\0'))
                                  .Where(ch => ch != '\0')
                                  .ToArray();
            var slug = new string(chars);
            while (slug.Contains("--")) slug = slug.Replace("--", "-");
            return slug.Trim('-');
        }

        private static string EnsureUniqueSlug(string baseSlug, HashSet<string> existing)
        {
            var slug = baseSlug;
            var suffix = 1;
            while (existing.Contains(slug))
            {
                suffix++;
                slug = $"{baseSlug}-{suffix}";
            }
            return slug;
        }
    }
}


