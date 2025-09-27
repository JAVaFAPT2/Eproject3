import React, { useState } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import {
  Box,
  Button,
  Flex,
  FormControl,
  FormLabel,
  Heading,
  Icon,
  Input,
  InputGroup,
  InputRightElement,
  Text,
  useColorModeValue,
  useToast,
} from '@chakra-ui/react';
import DefaultAuth from 'layouts/auth/Default';
import illustration from 'assets/img/auth/auth.png';
import { MdOutlineRemoveRedEye } from 'react-icons/md';
import { RiEyeCloseLine } from 'react-icons/ri';
import AuthService from 'services/AuthService';

function SignUp() {
  const textColor = useColorModeValue('navy.700', 'white');
  const textColorSecondary = 'gray.400';
  const textColorBrand = useColorModeValue('brand.500', 'white');
  const brandStars = useColorModeValue('brand.500', 'brand.400');

  const navigate = useNavigate();
  const toast = useToast();

  const [showPassword, setShowPassword] = useState(false);
  const [fullName, setFullName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleShowPassword = () => setShowPassword(!showPassword);

  const showToast = (title, status) => {
    toast({
      title,
      status,
      duration: 3000,
      isClosable: true,
      position: 'bottom-right',
    });
  };

  const handleEmailPasswordSignUp = async () => {
    if (password !== confirmPassword) {
      showToast('Passwords do not match', 'error');
      return;
    }
    try {
      await AuthService.signUp({ fullName, email, password });
      showToast('Account created successfully!', 'success');
      setTimeout(() => navigate('/auth/sign-in'), 1500);
    } catch (error) {
      console.error('Sign up failed:', error);
      showToast('Sign up failed. Please try again.', 'error');
    }
  };

  return (
    <DefaultAuth illustrationBackground={illustration} image={illustration}>
      <Flex
        maxW={{ base: '100%', md: 'max-content' }}
        w="100%"
        mx={{ base: 'auto', lg: '0px' }}
        me="auto"
        h="100%"
        alignItems="start"
        justifyContent="center"
        mb={{ base: '30px', md: '60px' }}
        px={{ base: '25px', md: '0px' }}
        mt={{ base: '40px', md: '5vh' }}
        flexDirection="column"
      >
        <Box me="auto">
          <Heading color={textColor} fontSize="36px" mb="10px">
            Sign Up
          </Heading>
          <Text
            mb="36px"
            ms="4px"
            color={textColorSecondary}
            fontWeight="400"
            fontSize="md"
          >
            Create an account to get started!
          </Text>
        </Box>

        <Flex
          zIndex="2"
          direction="column"
          w={{ base: '100%', md: '420px' }}
          maxW="100%"
          background="transparent"
          borderRadius="15px"
          mx={{ base: 'auto', lg: 'unset' }}
          me="auto"
          mb={{ base: '20px', md: 'auto' }}
        >
          <FormControl>
            <FormLabel
              display="flex"
              ms="4px"
              fontSize="sm"
              fontWeight="500"
              color={textColor}
              mb="8px"
            >
              Full Name<Text color={brandStars}>*</Text>
            </FormLabel>
            <Input
              isRequired
              variant="auth"
              fontSize="sm"
              type="text"
              placeholder="Your full name"
              mb="24px"
              size="lg"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
            />

            <FormLabel
              display="flex"
              ms="4px"
              fontSize="sm"
              fontWeight="500"
              color={textColor}
              mb="8px"
            >
              Email<Text color={brandStars}>*</Text>
            </FormLabel>
            <Input
              isRequired
              variant="auth"
              fontSize="sm"
              type="email"
              placeholder="mail@example.com"
              mb="24px"
              size="lg"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />

            <FormLabel
              display="flex"
              ms="4px"
              fontSize="sm"
              fontWeight="500"
              color={textColor}
              mb="8px"
            >
              Password<Text color={brandStars}>*</Text>
            </FormLabel>
            <InputGroup size="md" mb="24px">
              <Input
                isRequired
                fontSize="sm"
                placeholder="Min. 8 characters"
                size="lg"
                type={showPassword ? 'text' : 'password'}
                variant="auth"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
              <InputRightElement display="flex" alignItems="center" mt="4px">
                <Icon
                  color={textColorSecondary}
                  _hover={{ cursor: 'pointer' }}
                  as={showPassword ? RiEyeCloseLine : MdOutlineRemoveRedEye}
                  onClick={handleShowPassword}
                />
              </InputRightElement>
            </InputGroup>

            <FormLabel
              display="flex"
              ms="4px"
              fontSize="sm"
              fontWeight="500"
              color={textColor}
              mb="8px"
            >
              Confirm Password<Text color={brandStars}>*</Text>
            </FormLabel>
            <Input
              isRequired
              fontSize="sm"
              placeholder="Confirm password"
              size="lg"
              type={showPassword ? 'text' : 'password'}
              variant="auth"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />

            <Button
              fontSize="sm"
              variant="brand"
              fontWeight="500"
              w="100%"
              h="50"
              my="24px"
              onClick={handleEmailPasswordSignUp}
            >
              Sign Up
            </Button>
          </FormControl>

          <Flex
            flexDirection="column"
            justifyContent="center"
            alignItems="start"
            maxW="100%"
            mt="0px"
          >
            <Text color={textColorSecondary} fontWeight="400" fontSize="14px">
              Already have an account?
              <NavLink to="/auth/sign-in">
                <Text
                  color={textColorBrand}
                  as="span"
                  ms="5px"
                  fontWeight="500"
                >
                  Sign In
                </Text>
              </NavLink>
            </Text>
          </Flex>
        </Flex>
      </Flex>
    </DefaultAuth>
  );
}

export default SignUp;
