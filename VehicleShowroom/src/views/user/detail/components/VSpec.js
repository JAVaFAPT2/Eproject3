import { VStack, Text, HStack } from '@chakra-ui/react';

export default function VSpec({ head, sub }) {
  if (!sub) return null;

  // 🧩 Xử lý phần giá trị — nếu có nhiều phần (vd: "220 kW / 300 PS"), tách từng cụm
  const parts = sub.split('/').map((p) => p.trim());

  return (
    <VStack align="start" spacing={1} w="full">
      {/* 🔹 Dòng giá trị chính */}
      <HStack align="baseline" spacing={3}>
        {parts.map((part, idx) => {
          // Tách phần số và phần đơn vị (nếu có)
          const match = part.match(/^([\d.,+-]+)\s*(.*)$/);
          const value = match ? match[1] : part;
          const unit = match ? match[2] : '';

          return (
            <HStack key={idx} align="baseline" spacing={1}>
              <Text
                color="black"
                fontSize="7xl"
                lineHeight="short"
                fontFamily="'Arial Narrow', Arial, 'Heiti SC', SimHei, sans-serif"
                letterSpacing="-0.5px"
              >
                {value}
              </Text>
              {unit && (
                <Text fontSize="2xl" fontWeight="500">
                  {unit}
                </Text>
              )}
              {idx < parts.length - 1 && (
                <Text fontSize="3xl" fontWeight="500" mx={2}>
                  /
                </Text>
              )}
            </HStack>
          );
        })}
      </HStack>

      {/* 🔹 Tiêu đề spec */}
      <Text
        color="gray.500"
        fontSize="md"
        fontWeight="500"
      >
        {head}
      </Text>
    </VStack>
  );
}
