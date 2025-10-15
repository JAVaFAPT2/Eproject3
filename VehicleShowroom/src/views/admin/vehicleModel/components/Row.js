import React from 'react';
import {
  Tr,
  Td,
  Flex,
  Text,
  IconButton,
  useColorModeValue,
} from '@chakra-ui/react';
import {
  MdAdd,
  MdEdit,
  MdDelete,
  MdExpandMore,
  MdExpandLess,
} from 'react-icons/md';

export default function Row({
  model,
  depth = 0,
  expandedRows,
  toggleExpand,
  onAdd,
  onEdit,
  onDelete,
  renderChildren,
}) {
  const isExpanded = expandedRows[model.modelNumber];

  return (
    <>
      <Tr _hover={{ bg: useColorModeValue('gray.50', 'whiteAlpha.50') }}>
        <Td>
          <Flex align="center" pl={depth * 3}>
            <Text fontWeight="600">{model.modelNumber}</Text>
          </Flex>
        </Td>
        <Td onClick={() => toggleExpand(model.modelNumber)}>
          <Flex align="center">
            <Text>{model.name}</Text>
            {model.children?.length > 0 && (
              <IconButton
                aria-label="expand"
                size="sm"
                variant="ghost"
                icon={isExpanded ? <MdExpandLess /> : <MdExpandMore />}
              />
            )}
          </Flex>
        </Td>
        <Td>${model.price?.toLocaleString() || 0}</Td>
        <Td>{model.level}</Td>
        <Td textAlign="right">
          <Flex justify="flex-end" gap={2}>
            <IconButton
              aria-label="Add Submodel"
              size="sm"
              icon={<MdAdd />}
              colorScheme="green"
              borderRadius="xl"
              onClick={() => onAdd(model)}
            />
            <IconButton
              aria-label="Edit"
              size="sm"
              icon={<MdEdit />}
              colorScheme="blue"
              borderRadius="xl"
              onClick={() => onEdit(model)}
            />
            <IconButton
              aria-label="Delete"
              size="sm"
              icon={<MdDelete />}
              colorScheme="red"
              borderRadius="xl"
              onClick={() => onDelete(model)}
            />
          </Flex>
        </Td>
      </Tr>

      {isExpanded &&
        model.children?.map((child) => renderChildren(child, depth + 1))}
    </>
  );
}
