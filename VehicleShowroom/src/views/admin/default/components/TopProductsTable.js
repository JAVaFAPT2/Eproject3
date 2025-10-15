import React from 'react';
import {
  Box,
  Flex,
  Table,
  Tbody,
  Td,
  Text,
  Th,
  Thead,
  Tr,
  useColorModeValue,
  Spinner,
  Image,
  HStack,
} from '@chakra-ui/react';
import Card from 'components/card/Card';
import { formatUSD } from 'utils/FormatHelper';

export default function TopProductsTable({ products }) {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'whiteAlpha.100');

  if (!products || products.length === 0) {
    return (
      <Card justify="center" align="center" h="260px">
        <Spinner />
      </Card>
    );
  }

  return (
    <Card flexDirection="column" w="100%" px="0px" overflowX="auto">
      <Flex px="25px" mb="8px" justifyContent="space-between" align="center">
        <Text color={textColor} fontSize="22px" fontWeight="700">
          🛍️ Top 5 Products Sold
        </Text>
      </Flex>
      <Box>
        <Table variant="simple" color="gray.500" mb="24px">
          <Thead>
            <Tr>
              <Th borderColor={borderColor}>Product</Th>
              <Th borderColor={borderColor}>Sold</Th>
              <Th borderColor={borderColor}>Revenue</Th>
            </Tr>
          </Thead>
          <Tbody>
            {products.map((p, idx) => (
              <Tr key={idx}>
                <Td>
                  <HStack spacing={3}>
                    <Image
                      src={p.thumbnailUrl}
                      alt={p.productName}
                      boxSize="40px"
                      borderRadius="md"
                      objectFit="cover"
                    />
                    <Text fontWeight="600" color={textColor}>
                      {p.productName}
                    </Text>
                  </HStack>
                </Td>
                <Td>{p.sold}</Td>
                <Td>{formatUSD(p.revenue)}</Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </Box>
    </Card>
  );
}
