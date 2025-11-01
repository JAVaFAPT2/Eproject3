/* eslint-disable */
import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import {
  Box,
  Flex,
  HStack,
  Text,
  useColorModeValue,
  Icon,
} from '@chakra-ui/react';
import { MdArrowBack } from 'react-icons/md';

export function SidebarLinks(props) {
  const location = useLocation();
  const navigate = useNavigate();
  const { routes } = props;

  // 🎨 Chakra color mode
  const activeColor = useColorModeValue('gray.700', 'white');
  const inactiveColor = useColorModeValue(
    'secondaryGray.600',
    'secondaryGray.600',
  );
  const activeIcon = useColorModeValue('brand.500', 'white');
  const textColor = useColorModeValue('secondaryGray.500', 'white');
  const brandColor = useColorModeValue('brand.500', 'brand.400');

  // 🧭 Check route active
  const activeRoute = (routeName) => location.pathname.includes(routeName);

  // 🧱 Render route items
  const createLinks = (routes) =>
    routes.map((route, index) => {
      if (route.category) {
        return (
          <React.Fragment key={index}>
            <Text
              fontSize={'md'}
              color={activeColor}
              fontWeight="bold"
              mx="auto"
              ps={{ sm: '10px', xl: '16px' }}
              pt="18px"
              pb="12px"
            >
              {route.name}
            </Text>
            {createLinks(route.items)}
          </React.Fragment>
        );
      } else if (route.layout === '/admin') {
        return (
          <Box
            key={index}
            cursor="pointer"
            onClick={() => navigate(route.layout + route.path)}
          >
            <HStack
              spacing={
                activeRoute(route.path.toLowerCase()) ? '22px' : '26px'
              }
              py="5px"
              ps="10px"
            >
              <Flex w="100%" alignItems="center" justifyContent="center">
                <Box
                  color={
                    activeRoute(route.path.toLowerCase())
                      ? activeIcon
                      : textColor
                  }
                  me="18px"
                  display="flex"
                >
                  {route.icon}
                </Box>
                <Text
                  me="auto"
                  color={
                    activeRoute(route.path.toLowerCase())
                      ? activeColor
                      : textColor
                  }
                  fontWeight={
                    activeRoute(route.path.toLowerCase()) ? 'bold' : 'normal'
                  }
                >
                  {route.name}
                </Text>
              </Flex>
              <Box
                h="36px"
                w="4px"
                bg={
                  activeRoute(route.path.toLowerCase())
                    ? brandColor
                    : 'transparent'
                }
                borderRadius="5px"
              />
            </HStack>
          </Box>
        );
      }
      return null;
    });

  return (
    <Box>
      {createLinks(routes)}

      {/* 🔙 Back to Website Section */}
      <Box
        mt="6"
        px="12px"
        pt="20px"
        borderTop="1px solid"
        borderColor={useColorModeValue('gray.200', 'gray.700')}
      >
        <Flex
          align="center"
          gap="3"
          cursor="pointer"
          py="6px"
          px="10px"
          borderRadius="8px"
          _hover={{ bg: useColorModeValue('gray.100', 'whiteAlpha.100') }}
          onClick={() => navigate('/user/home')}
        >
          <Icon as={MdArrowBack} w="20px" h="20px" color={brandColor} />
          <Text color={brandColor} fontWeight="600">
            Back to Website
          </Text>
        </Flex>
      </Box>
    </Box>
  );
}

export default SidebarLinks;
