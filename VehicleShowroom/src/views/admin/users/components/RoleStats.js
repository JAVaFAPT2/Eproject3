import React, { useEffect, useState } from 'react';
import { SimpleGrid, Icon, useColorModeValue } from '@chakra-ui/react';
import MiniStatistics from 'components/card/MiniStatistics';
import IconBox from 'components/icons/IconBox';
import RoleService from 'services/RoleService';
import {
  MdPerson,
  MdAdminPanelSettings,
  MdBarChart,
  MdPerson2,
} from 'react-icons/md';

const roleIcons = {
  ADMIN: MdAdminPanelSettings,
  EMPLOYEE: MdPerson,
  USER: MdPerson2,
  DEFAULT: MdBarChart,
};

export default function RoleStats() {
  const brandColor = useColorModeValue('brand.500', 'white');
  const boxBg = useColorModeValue('secondaryGray.300', 'whiteAlpha.100');

  const [stats, setStats] = useState([]);

  useEffect(() => {
    loadStats();
  }, []);

  const loadStats = async () => {
    try {
      const roleStats = await RoleService.getRoleStats();
      setStats(roleStats || []);
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <SimpleGrid
      columns={{ base: 1, md: 3, lg: 3, '2xl': 3 }}
      gap="20px"
      mb="20px"
    >
      {stats.map((role) => {
        const IconComponent =
          roleIcons[role.name.toUpperCase()] || roleIcons.DEFAULT;

        return (
          <MiniStatistics
            key={role.id}
            startContent={
              <IconBox
                w="56px"
                h="56px"
                bg={boxBg}
                icon={
                  <Icon
                    w="32px"
                    h="32px"
                    as={IconComponent}
                    color={brandColor}
                  />
                }
              />
            }
            name={role.name}
            value={role.userCount}
          />
        );
      })}
    </SimpleGrid>
  );
}
