import React, { useEffect, useState } from 'react';
import { Box, SimpleGrid, Spinner } from '@chakra-ui/react';
import Card from 'views/user/list/components/Card';
import VehicleModelService from 'services/VehicleModelService';

export default function Section({ vehicles }) {
  const [models, setModels] = useState([]);
  const [loading, setLoading] = useState(true);

  // 🧩 Lấy danh sách Vehicle Models cấp 1 (level = 1)
  useEffect(() => {
    const fetchModels = async () => {
      try {
        const res = await VehicleModelService.grt({
          pageNumber: 1,
          pageSize: 100,
        });

        // chỉ lấy model cấp 1
        const level1Models = (res.data || []).filter((m) => m.level === 1);
        setModels(level1Models);
      } catch (err) {
        console.error('Error loading vehicle models', err);
      } finally {
        setLoading(false);
      }
    };

    fetchModels();
  }, []);

  if (loading) {
    return (
      <Box textAlign="center" py={10}>
        <Spinner size="lg" />
      </Box>
    );
  }

  if (!vehicles || vehicles.length === 0) {
    return (
      <Box color="gray.500" fontStyle="italic" py={10}>
        No vehicles available for the selected filters.
      </Box>
    );
  }

  return (
    <Box>
      {/* 🔹 Danh sách xe */}
      <SimpleGrid columns={{ base: 1, sm: 2, md: 3 }} spacing={8}>
        {vehicles.map((item) => {
          const modelInfo = models.find(
            (m) =>
              item.modelNumber &&
              item.modelNumber.toLowerCase() ===
                m.modelNumber.toLowerCase(),
          );

          return (
            <Card
              key={item.vehicleId}
              item={{
                ...item,
                modelName: modelInfo?.name || item.modelNumber,
                description: modelInfo?.description || '',
              }}
            />
          );
        })}
      </SimpleGrid>
    </Box>
  );
}
