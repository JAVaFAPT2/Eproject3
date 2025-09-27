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
import UserService from 'services/UserService';
import ProfileTab from './components/ProfileTab';
import PasswordTab from './components/PasswordTab';
import DeleteAccountTab from './components/DeleteAccountTab';

export default function ProfilePage() {
  const [user, setUser] = useState(null);

  const bgColor = useColorModeValue('white', 'navy.800');
  const brandColor = useColorModeValue('brand.500', 'brand.400');
  const borderColor = useColorModeValue('rgba(11,20,55,0.1)', 'navy.600');

  useEffect(() => {
    const fetchUser = async () => {
      const profile = await UserService.getProfile();
      setUser(profile);
    };
    fetchUser();
  }, []);

  return (
    <Box my="50px" w="80%" mx="auto">
      <Tabs
        variant="unstyled"
        orientation="vertical"
        isFitted={false}
        display="flex"
        border="1px solid"
        borderColor={borderColor}
        borderRadius="lg"
        overflow="hidden"
        shadow="lg"
        minH="600px"
      >
        <Flex w={'100%'} display={{ sm: 'block', md: 'flex' }}>
          <TabList
            flexDirection="column"
            w="220px"
            borderRight="1px solid"
            borderColor={borderColor}
            bg={bgColor}
          >
            {['Profile', 'Change Password', 'Delete Account'].map((label) => (
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

          <TabPanels w={'100%'} p={6} bg={bgColor}>
            <TabPanel>{user && <ProfileTab user={user} />}</TabPanel>
            <TabPanel>
              <PasswordTab />
            </TabPanel>
            <TabPanel>
              <DeleteAccountTab />
            </TabPanel>
          </TabPanels>
        </Flex>
      </Tabs>
    </Box>
  );
}
