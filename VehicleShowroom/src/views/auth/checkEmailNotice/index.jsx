import React from 'react';
import { Box, Button, Flex, Heading, Text, useColorModeValue } from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import DefaultAuth from 'layouts/auth/Default';
import illustration from 'assets/img/auth/auth.png';

function CheckEmailNotice() {
  const textColor = useColorModeValue('navy.700', 'white');
  const textColorSecondary = 'gray.400';
  const navigate = useNavigate();

  return (
    <DefaultAuth illustrationBackground={illustration} image={illustration}>
      <Flex
        maxW={{ base: '100%', md: 'max-content' }}
        w="100%"
        mx={{ base: 'auto', lg: '0px' }}
        me="auto"
        h="70vh"
        alignItems="center"
        justifyContent="center"
        flexDirection="column"
        px={{ base: '25px', md: '0px' }}
        mt={{ base: '40px', md: '10vh' }}
      >
        <Box textAlign="center">
          <Heading color={textColor} fontSize="32px" mb="10px">
            Check your email
          </Heading>
          <Text color={textColorSecondary} fontSize="md" mb="24px" w={{ base: '100%', md: '400px' }}>
            We have sent a password reset link to your email.  
            Please check your inbox and follow the instructions.
          </Text>
          <Button
            variant="brand"
            onClick={() => navigate('/auth/sign-in')}
          >
            Back to Sign In
          </Button>
        </Box>
      </Flex>
    </DefaultAuth>
  );
}

export default CheckEmailNotice;
