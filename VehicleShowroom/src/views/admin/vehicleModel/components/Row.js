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
  MdImage,
} from 'react-icons/md';
import { formatUSD } from 'utils/FormatHelper';

export default function Row({
  index,
  model,
  depth = 0,
  expandedRows,
  toggleExpand,
  onAdd,
  onAddSpec,
  onPreview,
  onEdit,
  onDelete,
  onEditSpec,
  onDeleteSpec,
  renderChildren,
}) {
  const isExpanded = expandedRows[model.modelNumber];
  const hasChildren = model.children?.length > 0;
  const hasSpecs = model.level === 2 && (model.specs?.length > 0 || true); // luôn có thể expand cấp 2 để load specs lần đầu

  return (
    <>
      <Tr _hover={{ bg: useColorModeValue('gray.50', 'whiteAlpha.50') }}>
        <Td textAlign="center" w="40px">
          <Text fontWeight="500">{index}</Text>
        </Td>
        <Td onClick={() => toggleExpand(model.modelNumber)} cursor="pointer">
          <Flex align="center" pl={depth * 3}>
            <Text fontWeight="600">{model.modelNumber}</Text>
            {/* 🔽 Expand icon cho cấp 1 hoặc cấp 2 */}
            {(hasChildren || hasSpecs) && (
              <IconButton
                aria-label="expand"
                size="sm"
                variant="ghost"
                icon={isExpanded ? <MdExpandLess /> : <MdExpandMore />}
                mr={2}
              />
            )}
          </Flex>
        </Td>

        <Td>
          <Flex align="center">
            <Text>{model.name}</Text>
          </Flex>
        </Td>

        <Td>{formatUSD(model.price)}</Td>

        <Td textAlign="right">
          <Flex justify="flex-end" gap={2}>
            <IconButton
              aria-label="Add Submodel or Spec"
              size="sm"
              icon={<MdAdd />}
              colorScheme="green"
              borderRadius="xl"
              onClick={() =>
                model.level < 2 ? onAdd(model) : onAddSpec(model)
              }
            />
            <IconButton
              aria-label="Preview Photos"
              size="sm"
              icon={<MdImage />}
              colorScheme="purple"
              borderRadius="xl"
              onClick={() => onPreview(model)}
            />
            <IconButton
              aria-label="Edit Model"
              size="sm"
              icon={<MdEdit />}
              colorScheme="blue"
              borderRadius="xl"
              onClick={() => onEdit(model)}
            />
            <IconButton
              aria-label="Delete Model"
              size="sm"
              icon={<MdDelete />}
              colorScheme="red"
              borderRadius="xl"
              onClick={() => onDelete(model)}
            />
          </Flex>
        </Td>
      </Tr>

      {/* 🔹 Cấp con (submodel) */}
      {isExpanded &&
        model.children?.map((child, i) => renderChildren(child, i, depth + 1))}

      {/* 🔹 Specifications cho model cấp 2 */}
      {isExpanded &&
        model.level === 2 &&
        model.specs?.length > 0 &&
        model.specs.map((spec, idx) => (
          <Tr key={`${model.modelNumber}-spec-${idx}`}>
            <Td pl={depth * 3 + 6}>
              <Text fontSize="sm" fontWeight="500" color="gray.600">
                {`${index}.${String.fromCharCode(97 + idx)}`}
              </Text>
            </Td>
            <Td>
              <Text fontSize="sm">{spec.groupName || '-'}</Text>
            </Td>
            <Td colSpan={2}>
              <Text fontSize="sm">
                <b>{spec.specName}</b>: {spec.specValue}
              </Text>
            </Td>
            <Td textAlign="right">
              <Flex justify="flex-end" gap={2}>
                <IconButton
                  aria-label="Edit Spec"
                  size="sm"
                  icon={<MdEdit />}
                  colorScheme="blue"
                  borderRadius="xl"
                  onClick={() => onEditSpec(spec)}
                />
                <IconButton
                  aria-label="Delete Spec"
                  size="sm"
                  icon={<MdDelete />}
                  colorScheme="red"
                  borderRadius="xl"
                  onClick={() => onDeleteSpec(spec)}
                />
              </Flex>
            </Td>
          </Tr>
        ))}
    </>
  );
}
