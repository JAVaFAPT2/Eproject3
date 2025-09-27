import {
  Box,
  Flex,
  Image,
  Link,
  Text,
  useColorModeValue,
  Icon,
} from '@chakra-ui/react';
import Card from 'components/card/Card.js';
import React from 'react';
import { ChevronRightIcon } from '@chakra-ui/icons';

export default function ProductCard(props) {
  const { image, name, author, price, link } = props;
  const textColor = useColorModeValue('navy.700', 'white');
  const textColorPrice = useColorModeValue('brand.500', 'white');
  const textColorBrand = useColorModeValue('brand.500', 'brand.400');

  return (
    <Card p="20px">
      <Flex direction={{ base: 'column' }} justify="center">
        {/* Hình sản phẩm */}
        <Box mb={{ base: '20px', '2xl': '20px' }} position="relative">
          <Image
            src={image}
            w="100%"
            h="400px"
            borderRadius="20px"
            objectFit="cover"
          />
        </Box>

        {/* Nội dung sản phẩm */}
        <Flex flexDirection="column" justify="space-between" h="100%">
          <Flex
            justify="space-between"
            direction={{
              base: 'row',
              md: 'column',
              lg: 'row',
              xl: 'column',
              '2xl': 'row',
            }}
            mb="auto"
          >
            <Flex direction="column">
              <Text
                color={textColor}
                fontSize={{
                  base: 'xl',
                  md: 'lg',
                  lg: 'lg',
                  xl: 'lg',
                  '2xl': 'md',
                  '3xl': 'lg',
                }}
                mb="5px"
                fontWeight="bold"
                me="14px"
              >
                {name}
              </Text>
              <Text
                color="secondaryGray.600"
                fontSize={{
                  base: 'sm',
                }}
                fontWeight="400"
                me="14px"
              >
                {author}
              </Text>
            </Flex>
          </Flex>

          {/* Giá + link chi tiết */}
          <Flex
            align="start"
            justify="space-between"
            direction={{
              base: 'row',
              md: 'column',
              lg: 'row',
              xl: 'column',
              '2xl': 'row',
            }}
            mt="25px"
          >
            <Text fontWeight="700" fontSize="sm" color={textColorPrice}>
              Price:{' '}
              {new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD',
              }).format(price)}
            </Text>

            <Link
              href={link}
              mt={{
                base: '0px',
                md: '10px',
                lg: '0px',
                xl: '10px',
                '2xl': '0px',
              }}
            >
              <Flex align="center" gap={1}>
                <Text fontSize="sm" color="brand.500" fontWeight="500">
                  View details
                </Text>
                <Icon as={ChevronRightIcon} color={textColorBrand} />
              </Flex>
            </Link>
          </Flex>
        </Flex>
      </Flex>
    </Card>
  );
}
