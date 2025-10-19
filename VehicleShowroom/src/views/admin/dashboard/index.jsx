import React, { useEffect, useState, useMemo } from 'react';
import {
  Box,
  SimpleGrid,
  Flex,
  Text,
  useColorModeValue,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  Badge,
  Icon,
} from '@chakra-ui/react';
import {
  MdAttachMoney,
  MdShoppingCart,
  MdPeople,
  MdWork,
  MdInventory,
  MdCategory,
} from 'react-icons/md';
import IconBox from 'components/icons/IconBox';
import MiniStatistics from 'components/card/MiniStatistics';
import Card from 'components/card/Card';
import LineChart from 'components/charts/LineChart';
import DashboardService from 'services/DashboardService';
import { formatCompact, formatUSD } from 'utils/FormatHelper';

export default function DashboardPage() {
  const brandColor = useColorModeValue('brand.500', 'white');
  const boxBg = useColorModeValue('secondaryGray.300', 'whiteAlpha.100');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const textColorSecondary = useColorModeValue('secondaryGray.600', 'gray.400');

  // ⚙️ State
  const [overview, setOverview] = useState(null);
  const [revenue, setRevenue] = useState(null);
  const [customer, setCustomer] = useState(null);
  const [topVehicles, setTopVehicles] = useState([]);
  const [recentOrders, setRecentOrders] = useState([]);

  // 🚀 Load dashboard
  useEffect(() => {
    const loadAll = async () => {
      try {
        const [ov, rev, cus, top, orders] = await Promise.all([
          DashboardService.getOverview(),
          DashboardService.getRevenue(),
          DashboardService.getCustomer(),
          DashboardService.getTopVehicles(),
          DashboardService.getRecentOrders(),
        ]);

        setOverview(ov);
        setRevenue(rev);
        setCustomer(cus);
        setTopVehicles(top);
        setRecentOrders(orders);
      } catch (err) {
        console.error('❌ Failed to load dashboard:', err);
      } finally {
      }
    };
    loadAll();
  }, []);

  // 🧠 Revenue chart data
  const revenueChart = useMemo(() => {
    if (!revenue?.revenueData) return { series: [], options: {} };
    const months = revenue.revenueData.map((x) => x.label);
    const values = revenue.revenueData.map((x) => x.value);
    return {
      series: [{ name: 'Revenue', data: values }],
      options: {
        chart: { type: 'line', toolbar: { show: false } },
        stroke: { curve: 'smooth', width: 3 },
        colors: ['#7A7A7A'],
        xaxis: {
          categories: months,
          labels: { style: { colors: textColorSecondary } },
        },
        yaxis: { labels: { style: { colors: textColorSecondary } } },
        grid: { borderColor: '#E2E8F0', strokeDashArray: 4 },
        tooltip: { y: { formatter: (val) => formatUSD(val) } },
      },
    };
  }, [revenue, textColorSecondary]);

  // 🧠 Customer chart data
  const customerChart = useMemo(() => {
    if (!customer?.customerGrowthData) return { series: [], options: {} };
    const months = customer.customerGrowthData.map((x) => x.label);
    const values = customer.customerGrowthData.map((x) => x.newCustomers);
    return {
      series: [{ name: 'New Customers', data: values }],
      options: {
        chart: { type: 'line', toolbar: { show: false } },
        stroke: { curve: 'smooth', width: 3 },
        colors: ['#7A7A7A'],
        xaxis: {
          categories: months,
          labels: { style: { colors: textColorSecondary } },
        },
        yaxis: { labels: { style: { colors: textColorSecondary } } },
        grid: { borderColor: '#E2E8F0', strokeDashArray: 4 },
      },
    };
  }, [customer, textColorSecondary]);

  return (
    <>
      {/* 1️⃣ Overview Cards */}
      <SimpleGrid
        columns={{ base: 1, md: 2, lg: 3, '2xl': 6 }}
        gap="20px"
        mb="20px"
      >
        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={
                <Icon as={MdAttachMoney} w="32px" h="32px" color={brandColor} />
              }
            />
          }
          name="Profit"
          value={formatCompact(overview?.profit || 0)}
        />
        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={<Icon as={MdWork} w="32px" h="32px" color={brandColor} />}
            />
          }
          name="Employees"
          value={overview?.employees || 0}
        />
        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={<Icon as={MdPeople} w="32px" h="32px" color={brandColor} />}
            />
          }
          name="Customers Purchased"
          value={overview?.customersPurchased || 0}
        />
        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={
                <Icon
                  as={MdShoppingCart}
                  w="32px"
                  h="32px"
                  color={brandColor}
                />
              }
            />
          }
          name="Completed Orders"
          value={overview?.completedOrders || 0}
        />
        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={
                <Icon as={MdInventory} w="32px" h="32px" color={brandColor} />
              }
            />
          }
          name="Vehicles"
          value={overview?.vehicles || 0}
        />
        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={
                <Icon as={MdCategory} w="32px" h="32px" color={brandColor} />
              }
            />
          }
          name="Models"
          value={overview?.level2Models || 0}
        />
      </SimpleGrid>

      {/* 2️⃣ Revenue & Customers */}
      <SimpleGrid columns={{ base: 1, md: 2 }} gap="20px" mb="20px">
        <Card p="20px">
          <Flex justify="space-between" align="center" mb="4">
            <Text fontSize="xl" fontWeight="700" color={textColor}>
              Revenue
            </Text>
            <Badge
              colorScheme={revenue?.growthPercentage >= 0 ? 'green' : 'red'}
            >
              {revenue?.growthPercentage?.toFixed(1) || 0}%
            </Badge>
          </Flex>
          <Text fontSize="3xl" fontWeight="700">
            {formatUSD(revenue?.totalRevenue || 0)}
          </Text>
          <Text fontSize="sm" color={textColorSecondary}>
            This month • Avg Order: {formatUSD(revenue?.averageOrderValue || 0)}
          </Text>
          <Box mt="4" minH="220px">
            <LineChart
              series={revenueChart.series}
              options={revenueChart.options}
            />
          </Box>
        </Card>

        <Card p="20px">
          <Flex justify="space-between" align="center" mb="4">
            <Text fontSize="xl" fontWeight="700" color={textColor}>
              Customers
            </Text>
            <Badge
              colorScheme={
                customer?.customerGrowthPercentage >= 0 ? 'green' : 'red'
              }
            >
              {customer?.customerGrowthPercentage?.toFixed(1) || 0}%
            </Badge>
          </Flex>
          <Text fontSize="3xl" fontWeight="700">
            {customer?.newCustomers || 0}
          </Text>
          <Text fontSize="sm" color={textColorSecondary}>
            New this month • Active: {customer?.activeCustomers || 0}
          </Text>
          <Box mt="4" minH="220px">
            <LineChart
              series={customerChart.series}
              options={customerChart.options}
            />
          </Box>
        </Card>
      </SimpleGrid>

      {/* 3️⃣ Top Vehicles & Recent Orders */}
      <SimpleGrid columns={{ base: 1, md: 2 }} gap="20px">
        <Card p="20px">
          <Text fontSize="xl" fontWeight="700" mb="4">
            Top Vehicles
          </Text>
          <Table size="sm" variant="simple">
            <Thead>
              <Tr>
                <Th>Model</Th>
                <Th isNumeric>Sales</Th>
                <Th isNumeric>Revenue</Th>
              </Tr>
            </Thead>
            <Tbody>
              {topVehicles?.map((v) => (
                <Tr key={v.modelNumber}>
                  <Td>{v.model}</Td>
                  <Td isNumeric>{v.salesCount}</Td>
                  <Td isNumeric>{formatUSD(v.totalRevenue)}</Td>
                </Tr>
              ))}
            </Tbody>
          </Table>
        </Card>

        {/* ✅ Recent Orders - hiển thị Customer Name */}
        <Card p="20px">
          <Text fontSize="xl" fontWeight="700" mb="4">
            Recent Orders
          </Text>
          <Table size="sm" variant="simple">
            <Thead>
              <Tr>
                <Th>Customer</Th>
                <Th>Model</Th>
                <Th isNumeric>Amount</Th>
                <Th>Status</Th>
              </Tr>
            </Thead>
            <Tbody>
              {recentOrders?.map((o) => (
                <Tr key={o.orderId}>
                  <Td>{o.customerName || 'Unknown'}</Td>
                  <Td>{o.vehicleModel}</Td>
                  <Td isNumeric>{formatUSD(o.totalAmount)}</Td>
                  <Td>
                    <Badge
                      colorScheme={o.status === 'Completed' ? 'green' : 'gray'}
                    >
                      {o.status}
                    </Badge>
                  </Td>
                </Tr>
              ))}
            </Tbody>
          </Table>
        </Card>
      </SimpleGrid>
    </>
  );
}
