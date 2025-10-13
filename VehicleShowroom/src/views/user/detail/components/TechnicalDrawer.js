import React, { useState } from 'react';
import {
  Box,
  Text,
  IconButton,
  Collapse,
  VStack,
  Drawer,
  DrawerOverlay,
  DrawerContent,
  DrawerHeader,
  DrawerBody,
  DrawerCloseButton,
  Flex,
} from '@chakra-ui/react';
import { AddIcon, MinusIcon } from '@chakra-ui/icons';
import HSpec from 'views/user/detail/components/HSpec';

export default function TechnicalDrawer({ isOpen, onClose, groupedSpecs }) {
  const [openGroups, setOpenGroups] = useState({});

  const toggleGroup = (group) => {
    setOpenGroups((prev) => ({ ...prev, [group]: !prev[group] }));
  };

  // Bỏ qua các group General / GeneralPerformance
  const visibleGroups = Object.keys(groupedSpecs).filter(
    (g) =>
      !g.toLowerCase().includes('general') &&
      groupedSpecs[g] &&
      groupedSpecs[g].length > 0,
  );

  return (
    <Drawer isOpen={isOpen} placement="right" onClose={onClose} size="md">
      <DrawerOverlay />
      <DrawerContent>
        <DrawerCloseButton />
        <DrawerHeader fontSize="2xl" fontWeight="700">
          Technical Details
        </DrawerHeader>
        <DrawerBody>
          <VStack align="stretch" spacing={4}>
            {visibleGroups.map((group) => (
              <Box
                key={group}
                borderBottom="1px solid"
                borderColor="gray.200"
                pb={3}
              >
                <Flex
                  justify="space-between"
                  align="center"
                  cursor="pointer"
                  onClick={() => toggleGroup(group)}
                >
                  <Text fontSize="xl" fontWeight="600">
                    {group}
                  </Text>
                  <IconButton
                    size="sm"
                    variant="ghost"
                    aria-label={`Toggle ${group}`}
                    icon={
                      openGroups[group] ? (
                        <MinusIcon boxSize={4} />
                      ) : (
                        <AddIcon boxSize={4} />
                      )
                    }
                  />
                </Flex>

                <Collapse in={openGroups[group]} animateOpacity>
                  <Box mt={2} pl={1}>
                    {groupedSpecs[group].map((spec) => (
                      <HSpec
                        key={spec.specId}
                        head={spec.specName}
                        sub={spec.specValue}
                      />
                    ))}
                  </Box>
                </Collapse>
              </Box>
            ))}
          </VStack>
        </DrawerBody>
      </DrawerContent>
    </Drawer>
  );
}
