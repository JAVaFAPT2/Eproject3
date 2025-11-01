import React, { useState, useEffect } from 'react';
import {
  Tabs,
  TabList,
  TabPanels,
  Tab,
  TabPanel,
  Box,
  useColorModeValue,
  Flex,
} from '@chakra-ui/react';
import ProfileService from 'services/ProfileService';
import OrderService from 'services/OrderService';
import ServiceOrderService from 'services/ServiceOrderService';
import ProfileTab from './components/ProfileTab';
import PasswordTab from './components/PasswordTab';
import OrderHistoryTab from './components/OrderHistoryTab';
import ServiceHistoryTab from './components/ServiceHistoryTab';
import { useAppToast } from 'utils/ToastHelper';

export default function ProfilePage() {
  const [user, setUser] = useState(null);
  const [orders, setOrders] = useState([]);
  const [services, setServices] = useState([]);
  const toast = useAppToast();

  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const sectionBg = useColorModeValue('gray.50', 'navy.700');
  const brandColor = useColorModeValue('brand.500', 'brand.400');
  const borderColor = useColorModeValue('rgba(11,20,55,0.1)', 'navy.600');

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await ProfileService.get();
        setUser(data);

        // ⚡ gọi API có truyền param customerId
        const [orderRes, serviceRes] = await Promise.all([
          OrderService.get({ customerId: data.id }),
          ServiceOrderService.get({ customerId: data.id }),
        ]);

        setOrders(orderRes.items || []);
        setServices(serviceRes.items || []);
      } catch (err) {
        console.error('❌ Failed to load profile or history:', err);
        toast.error('Failed to load profile or history');
      }
    };

    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <Box py="100px" w="80%" mx="auto">
      <Tabs
        variant="unstyled"
        orientation="vertical"
        display="flex"
        border="1px solid"
        borderColor={borderColor}
        borderRadius="lg"
        overflow="hidden"
        shadow="lg"
        minH="600px"
      >
        <Flex w="100%" display={{ sm: 'block', md: 'flex' }}>
          {/* 🟢 TabList: Đưa Order & Service lên trước */}
          <TabList
            flexDirection="column"
            w="220px"
            borderRight="1px solid"
            borderColor={borderColor}
            bg={bgColor}
          >
            {[
              'Order History',
              'Service History',
              'Profile',
              'Change Password',
            ].map((label) => (
              <Tab
                key={label}
                justifyContent="flex-start"
                px={6}
                py={4}
                fontWeight="medium"
                position="relative"
                _selected={{
                  color: brandColor,
                  fontWeight: 'bold',
                  _after: {
                    content: '""',
                    position: 'absolute',
                    right: '0',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    h: '30px',
                    borderRight: '2px solid',
                    borderColor: brandColor,
                  },
                }}
              >
                {label}
              </Tab>
            ))}
          </TabList>

          {/* 🟢 TabPanels tương ứng thứ tự mới */}
          <TabPanels w="100%" p={6} bg={bgColor}>
            <TabPanel>
              <OrderHistoryTab orders={orders} />
            </TabPanel>
            <TabPanel>
              <ServiceHistoryTab services={services} />
            </TabPanel>
            <TabPanel>
              {user && (
                <ProfileTab
                  user={user}
                  colors={{ bgColor, textColor, sectionBg }}
                />
              )}
            </TabPanel>
            <TabPanel>
              <PasswordTab colors={{ bgColor, textColor, sectionBg }} />
            </TabPanel>
          </TabPanels>
        </Flex>
      </Tabs>
    </Box>
  );
}
