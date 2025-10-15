import React, { useEffect, useState } from 'react';
import {
  Box,
  Flex,
  Icon,
  SimpleGrid,
  Spinner,
  useColorModeValue,
  Text,
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
import TotalRevenue from 'views/admin/default/components/TotalRevenue';
import TotalCustomers from 'views/admin/default/components/TotalCustomers';
import TopCategoryChart from 'views/admin/default/components/TopCategoryChart';
import TopProductsTable from 'views/admin/default/components/TopProductsTable';
import EmployeeStanding from 'views/admin/default/components/EmployeeStanding';
import ReportService from 'services/ReportService';

export default function UserReports() {
  const brandColor = useColorModeValue('brand.500', 'white');
  const boxBg = useColorModeValue('secondaryGray.300', 'whiteAlpha.100');

  // ⚡ State
  const [overview, setOverview] = useState(null);
  const [loadingOverview, setLoadingOverview] = useState(true);

  const [revenueSummary, setRevenueSummary] = useState(null);
  const [customerSummary, setCustomerSummary] = useState(null);

  const [revenueTrend, setRevenueTrend] = useState([]);
  const [customerTrend, setCustomerTrend] = useState([]);

  const [categoryWeekly, setCategoryWeekly] = useState([]);
  const [topProducts, setTopProducts] = useState([]);
  const [topEmployees, setTopEmployees] = useState([]);

  // ⚙️ Load Data
  useEffect(() => {
    const loadAll = async () => {
      try {
        const [
          overviewRes,
          revenueSummaryRes,
          customerSummaryRes,
          revenueTrendRes,
          customerTrendRes,
          categoryWeeklyRes,
          topProductsRes,
          topEmployeesRes,
        ] = await Promise.all([
          ReportService.getOverview(),
          ReportService.getRevenueSummary(),
          ReportService.getCustomerSummary(),
          ReportService.getRevenueTrend(),
          ReportService.getCustomerTrend(),
          ReportService.getTopCategoriesWeekly(),
          ReportService.getTopProductsMonthly(),
          ReportService.getTopEmployeesMonthly(),
        ]);

        setOverview(overviewRes.data);
        setRevenueSummary(revenueSummaryRes.data);
        setCustomerSummary(customerSummaryRes.data);
        setRevenueTrend(revenueTrendRes.data);
        setCustomerTrend(customerTrendRes.data);
        setCategoryWeekly(categoryWeeklyRes.data);
        setTopProducts(topProductsRes.data);
        setTopEmployees(topEmployeesRes.data);
      } catch (err) {
        console.error('❌ Failed to load dashboard data:', err);
      } finally {
        setLoadingOverview(false);
      }
    };

    loadAll();
  }, []);

  // ⏳ Loading
  if (loadingOverview) {
    return (
      <Flex justify="center" align="center" h="80vh" direction="column" gap={4}>
        <Spinner size="xl" />
        <Text>Loading dashboard data...</Text>
      </Flex>
    );
  }

  return (
    <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
      {/* 🧭 1️⃣ Tổng quan */}
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
                <Icon w="32px" h="32px" as={MdAttachMoney} color={brandColor} />
              }
            />
          }
          name="Total Revenue"
          value={`$${overview?.totalRevenue || 0}`}
        />

        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={
                <Icon
                  w="32px"
                  h="32px"
                  as={MdShoppingCart}
                  color={brandColor}
                />
              }
            />
          }
          name="Total Orders"
          value={overview?.totalOrders || 0}
        />

        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={<Icon w="32px" h="32px" as={MdPeople} color={brandColor} />}
            />
          }
          name="Total Customers"
          value={overview?.totalCustomers || 0}
        />

        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={<Icon w="32px" h="32px" as={MdWork} color={brandColor} />}
            />
          }
          name="Employees"
          value={overview?.totalEmployees || 0}
        />

        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={
                <Icon w="32px" h="32px" as={MdInventory} color={brandColor} />
              }
            />
          }
          name="Products"
          value={overview?.totalProducts || 0}
        />

        <MiniStatistics
          startContent={
            <IconBox
              w="56px"
              h="56px"
              bg={boxBg}
              icon={
                <Icon w="32px" h="32px" as={MdCategory} color={brandColor} />
              }
            />
          }
          name="Categories"
          value={overview?.totalCategories || 0}
        />
      </SimpleGrid>

      {/* 💰 2️⃣ Doanh thu + Khách hàng */}
      <SimpleGrid columns={{ base: 1, md: 2, xl: 2 }} gap="20px" mb="20px">
        <TotalRevenue summary={revenueSummary} trend={revenueTrend} />
        <TotalCustomers summary={customerSummary} trend={customerTrend} />
      </SimpleGrid>

      {/* 📊 3️⃣ Biểu đồ danh mục + bảng top */}
      <SimpleGrid columns={{ base: 1, md: 2 }} gap="20px" mb="20px">
        <TopCategoryChart data={categoryWeekly} />
        <TopProductsTable products={topProducts} />
      </SimpleGrid>

      {/* 👷 4️⃣ Nhân viên */}
      <Box>
        <EmployeeStanding employees={topEmployees} />
      </Box>
    </Box>
  );
}
