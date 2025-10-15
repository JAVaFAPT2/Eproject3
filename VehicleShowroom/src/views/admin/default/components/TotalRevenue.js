import React, { useMemo } from 'react';
import {
  Box,
  Button,
  Flex,
  Icon,
  Text,
  useColorModeValue,
  Spinner,
  Badge,
} from '@chakra-ui/react';
import Card from 'components/card/Card';
import LineChart from 'components/charts/LineChart';
import { MdOutlineCalendarToday } from 'react-icons/md';
import { formatUSD } from 'utils/FormatHelper';

export default function TotalRevenue({ summary, trend }) {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const textColorSecondary = useColorModeValue('secondaryGray.600', 'white');
  const boxBg = useColorModeValue('secondaryGray.300', 'whiteAlpha.100');
  const iconColor = useColorModeValue('brand.500', 'white');
  const gridColor = useColorModeValue('#E2E8F0', '#2D3748');

  const { series, options } = useMemo(() => {
    if (!trend || trend.length === 0)
      return {
        series: [{ name: 'Revenue', data: [] }],
        options: { xaxis: { categories: [] } },
      };

    const labels = trend.map((d) => `${d.month}/${d.year.toString().slice(2)}`);
    const revenues = trend.map((d) => d.value);

    return {
      series: [{ name: 'Revenue', data: revenues }],
      options: {
        chart: {
          type: 'line',
          toolbar: { show: false },
          zoom: { enabled: false },
        },
        stroke: { curve: 'smooth', width: 3 },
        colors: [iconColor],
        xaxis: {
          categories: labels,
          labels: { style: { colors: textColorSecondary } },
        },
        yaxis: { labels: { style: { colors: textColorSecondary } } },
        grid: { borderColor: gridColor, strokeDashArray: 4 },
        tooltip: { y: { formatter: (val) => formatUSD(val) } },
      },
    };
  }, [trend, iconColor, textColorSecondary]);

  if (!summary || !trend)
    return (
      <Card justify="center" align="center" h="260px">
        <Spinner />
      </Card>
    );

  return (
    <Card w="100%" h="full" p="20px">
      <Flex justify="space-between" align="center" mb="10px">
        <Button
          bg={boxBg}
          fontSize="sm"
          fontWeight="500"
          color={textColorSecondary}
          borderRadius="7px"
          leftIcon={<MdOutlineCalendarToday />}
        >
          Last 6 months
        </Button>
        <Badge
          colorScheme={summary.growthRate >= 0 ? 'green' : 'red'}
          borderRadius="full"
          px="3"
          py="1"
        >
          {summary.growthRate >= 0 ? '+' : ''}
          {summary.growthRate.toFixed(1)}%
        </Badge>
      </Flex>

      <Text color={textColor} fontSize="34px" fontWeight="700">
        {formatUSD(summary.currentMonthRevenue || 0)}
      </Text>
      <Text color={textColorSecondary} fontSize="sm" mb="20px">
        Revenue this month
      </Text>

      <Box minH="220px" w="full">
        <LineChart series={series} options={options} />
      </Box>
    </Card>
  );
}
