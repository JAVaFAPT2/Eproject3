import { Text, Flex, IconButton, Image } from '@chakra-ui/react';
import { MdEdit, MdDelete } from 'react-icons/md';
import { RiEyeFill } from 'react-icons/ri';
import { createColumnHelper } from '@tanstack/react-table';

const columnHelper = createColumnHelper();

export const getBlogColumns = ({
  onShowDetails,
  onEdit,
  onDelete,
  onPreviewImage,
}) => [
  columnHelper.accessor('thumbnail', {
    header: 'THUMBNAIL',
    cell: (info) => {
      const thumb = info.getValue();
      return thumb ? (
        <Image
          src={thumb}
          alt="Thumbnail"
          maxH="50px"
          borderRadius="md"
          cursor="pointer"
          onClick={() => onPreviewImage(thumb)}
        />
      ) : (
        <Text>No Image</Text>
      );
    },
  }),
  columnHelper.accessor('title', {
    header: 'TITLE',
    cell: (info) => (
      <Text fontSize="sm" fontWeight="600">
        {info.getValue()}
      </Text>
    ),
  }),
  columnHelper.accessor('content', {
    header: 'CONTENT',
    cell: (info) => (
      <Text
        noOfLines={2}
        maxW="300px"
        whiteSpace="nowrap"
        overflow="hidden"
        textOverflow="ellipsis"
      >
        {info.getValue() || '-'}
      </Text>
    ),
  }),
  columnHelper.accessor('createdAt', {
    header: 'CREATED AT',
    cell: (info) => <Text>{new Date(info.getValue()).toLocaleString()}</Text>,
  }),
  columnHelper.display({
    id: 'actions',
    header: () => <Text textAlign="right">ACTIONS</Text>,
    cell: (info) => {
      const blog = info.row.original;
      return (
        <Flex justify="flex-end" gap={2}>
          <IconButton
            aria-label="Show"
            size="sm"
            colorScheme="purple"
            icon={<RiEyeFill style={{ fontSize: '20px' }} />}
            borderRadius={10}
            onClick={() => onShowDetails(blog)}
          />
          <IconButton
            aria-label="Edit"
            size="sm"
            icon={<MdEdit style={{ fontSize: '20px' }} />}
            colorScheme="blue"
            borderRadius={10}
            onClick={() => onEdit(blog)}
          />
          <IconButton
            aria-label="Delete"
            size="sm"
            icon={<MdDelete style={{ fontSize: '20px' }} />}
            colorScheme="red"
            borderRadius={10}
            onClick={() => onDelete(blog)}
          />
        </Flex>
      );
    },
  }),
];
