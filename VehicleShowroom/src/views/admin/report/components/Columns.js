import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, Link, Icon } from '@chakra-ui/react';
import { FiDownload } from 'react-icons/fi';

const c = createColumnHelper();

export const getColumns = ({ textColor }) => [
  c.accessor('type', {
    header: 'REPORT TYPE',
    cell: (i) => <Text color={textColor}>{i.getValue()}</Text>,
  }),
  c.accessor('generatedAt', {
    header: 'GENERATED AT',
    cell: (i) => (
      <Text>{new Date(i.getValue()).toLocaleDateString()}</Text>
    ),
  }),
  c.accessor('summary', {
    header: 'SUMMARY',
    cell: (i) => <Text>{i.getValue()}</Text>,
  }),
  c.accessor('fileUrl', {
    header: () => (
      <Text textAlign="right" w="full">
        ACTIONS
      </Text>
    ),
    cell: (i) => {
      const fileUrl = i.getValue();
      return (
        <Flex justify="flex-end">
          {fileUrl ? (
            <Link href={fileUrl} isExternal color="blue.400" display="flex" alignItems="center" gap={1}>
              <Icon as={FiDownload} />
              Download
            </Link>
          ) : (
            <Text color="gray.400">N/A</Text>
          )}
        </Flex>
      );
    },
  }),
];
