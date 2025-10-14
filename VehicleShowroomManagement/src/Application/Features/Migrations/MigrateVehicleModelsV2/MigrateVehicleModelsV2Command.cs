using MediatR;

namespace VehicleShowroomManagement.Application.Features.Migrations.MigrateVehicleModelsV2
{
    /// <summary>
    /// Runs idempotent migration to normalize VehicleModel hierarchy (Level-1/Level-2) and generate slugs for Level-2.
    /// Returns summary counts of updated documents.
    /// </summary>
    public record MigrateVehicleModelsV2Command() : IRequest<MigrateVehicleModelsV2Result>;

    public class MigrateVehicleModelsV2Result
    {
        public int ModelsUpdated { get; set; }
        public int Level1Assigned { get; set; }
        public int Level2Assigned { get; set; }
        public int SlugsGenerated { get; set; }
    }
}


