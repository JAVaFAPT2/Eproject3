// Chakra imports
import { Box, Flex, Icon, Text } from '@chakra-ui/react';
import PropTypes from 'prop-types';
import React from 'react';
import Footer from 'components/footer/FooterAuth';
import FixedPlugin from 'components/fixedPlugin/FixedPlugin';
// Assets
import { FaChevronLeft } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';

function AuthIllustration(props) {
  const { children, illustrationBackground } = props;
  const navigate = useNavigate();
  // Chakra color mode
  return (
    <Flex position="relative" h="max-content">
      <Flex
        h={{
          sm: 'initial',
          md: 'unset',
          lg: '100vh',
          xl: '97vh',
        }}
        w="100%"
        maxW={{ md: '66%', lg: '1313px' }}
        mx="auto"
        px={{ lg: '30px', xl: '0px' }}
        ps={{ xl: '70px' }}
        justifyContent="start"
        direction="column"
      >
        <Flex
          align="center"
          ps={{ base: '25px', lg: '0px' }}
          pt="50px"
          w="fit-content"
          onClick={() => navigate(-1)}
          _hover={{ cursor: 'pointer' }}
        >
          <Icon
            as={FaChevronLeft}
            me="12px"
            h="13px"
            w="8px"
            color="secondaryGray.600"
          />
          <Text ms="0px" fontSize="md" color="secondaryGray.600">
            Back
          </Text>
        </Flex>
        {children}
        <Box
          display={{ base: 'none', md: 'block' }}
          h="100%"
          minH="100vh"
          w={{ lg: '50vw', '2xl': '44vw' }}
          position="absolute"
          right="0px"
        >
          <Flex w="100%" h="100%" position="relative">
            <Flex
              position="absolute"
              top={0}
              left={0}
              w="100%"
              h="100%"
              bg="linear-gradient(to bottom right, #4facfe, #7366ff, #d633ff, #ff6a00)"
              borderBottomLeftRadius={{ lg: '120px', xl: '200px' }}
              zIndex={0}
            />

            <Flex
              position="absolute"
              top={200}
              left={200}
              w="50%"
              h="50%"
              bgImage={`url(${illustrationBackground})`}
              bgSize="cover"
              bgPosition="center"
              zIndex={1}
              justify="center"
              align="end"
              display={{ lg: 'none', xl: 'flex' }}
            />
          </Flex>
        </Box>
        <Footer />
      </Flex>
      <FixedPlugin />
    </Flex>
  );
}
// PROPS

AuthIllustration.propTypes = {
  illustrationBackground: PropTypes.string,
  image: PropTypes.any,
};

export default AuthIllustration;
