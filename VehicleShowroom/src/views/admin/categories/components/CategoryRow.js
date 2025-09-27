import React from 'react';
import { Tr, Td, Flex, Text, IconButton } from '@chakra-ui/react';
import {
  MdAdd,
  MdEdit,
  MdDelete,
  MdExpandMore,
  MdExpandLess,
} from 'react-icons/md';

export default function CategoryRow({
  cat,
  depth = 0,
  expandedRows,
  toggleExpand,
  onAdd,
  onEdit,
  onDelete,
  renderChildren,
}) {
  const isExpanded = expandedRows[cat.id];

  return (
    <>
      <Tr>
        <Td>
          <Flex align="center" pl={depth * 3}>
            <Text fontSize="sm" fontWeight="600">
              {cat.name}
            </Text>
            {cat.children?.length > 0 && (
              <IconButton
                aria-label="expand"
                size="sm"
                variant="ghost"
                icon={isExpanded ? <MdExpandLess /> : <MdExpandMore />}
                onClick={() => toggleExpand(cat.id)}
              />
            )}
          </Flex>
        </Td>
        <Td>{cat.description || '-'}</Td>
        <Td>{new Date(cat.createdAt).toLocaleString()}</Td>
        <Td textAlign="right">
          <Flex justify="flex-end" gap={2}>
            <IconButton
              aria-label="Add Subcategory"
              size="sm"
              icon={<MdAdd />}
              colorScheme="green"
              borderRadius={10}
              onClick={() => onAdd(cat)}
            />
            <IconButton
              aria-label="Edit"
              size="sm"
              icon={<MdEdit />}
              colorScheme="blue"
              borderRadius={10}
              onClick={() => onEdit(cat)}
            />
            <IconButton
              aria-label="Delete"
              size="sm"
              icon={<MdDelete />}
              colorScheme="red"
              borderRadius={10}
              onClick={() => onDelete(cat)}
            />
          </Flex>
        </Td>
      </Tr>

      {isExpanded &&
        cat.children?.map((child) => renderChildren(child, depth + 1))}
    </>
  );
}
