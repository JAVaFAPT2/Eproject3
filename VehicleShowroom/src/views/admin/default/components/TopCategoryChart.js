import React, { useMemo } from 'react';
import {
  Box,
  Button,
  Flex,
  Icon,
  Text,
  useColorModeValue,
  Spinner,
} from '@chakra-ui/react';
import Card from 'components/card/Card';
import BarChart from 'components/charts/BarChart';
import { MdBarChart } from 'react-icons/md';

export default function TopCategoryChart({ data }) {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const iconColor = useColorModeValue('brand.500', 'white');
  const bgButton = useColorModeValue('secondaryGray.300', 'whiteAlpha.100');

  const chartData = useMemo(() => {
    if (!data || data.length === 0) return null;

    const labels = data.map((d) => d.categoryName);
    const revenues = data.map((d) => d.revenue);

    return {
      labels,
      datasets: [
        {
          label: 'Revenue',
          data: revenues,
          backgroundColor: iconColor,
        },
      ],
    };
  }, [data, iconColor]);

  return (
    <Card align="center" direction="column" w="100%">
      <Flex align="center" w="100%" px="15px" py="10px">
        <Text me="auto" color={textColor} fontSize="xl" fontWeight="700">
          Top Categories (Last 7 Days)
        </Text>
        <Button
          align="center"
          justifyContent="center"
          bg={bgButton}
          w="37px"
          h="37px"
          borderRadius="10px"
        >
          <Icon as={MdBarChart} color={iconColor} w="24px" h="24px" />
        </Button>
      </Flex>

      <Box
        h="240px"
        mt="auto"
        w="full"
        display="flex"
        justifyContent="center"
        alignItems="center"
      >
        {!chartData ? (
          <Spinner size="lg" color={iconColor} />
        ) : (
          <BarChart chartData={chartData} />
        )}
      </Box>
    </Card>
  );
}
