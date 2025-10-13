import React from 'react';
import { Box, SimpleGrid } from '@chakra-ui/react';
import Card from 'views/user/list/components/Card';
import { vehicleModels } from 'mockData/vehicleModels';

export default function Section({ vehicles }) {
  if (!vehicles || vehicles.length === 0) {
    return (
      <Box color="gray.500" fontStyle="italic">
        No vehicles available for the selected filters.
      </Box>
    );
  }

  return (
    <Box>
      {/* 🔹 Danh sách xe */}
      <SimpleGrid columns={{ base: 1, sm: 2, md: 3 }} spacing={8}>
        {vehicles.map((item) => {
          const modelInfo = vehicleModels.find((m) =>
            item.modelNumber
              .toLowerCase()
              .includes(m.modelNumber.toLowerCase()),
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
