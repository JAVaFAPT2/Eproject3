import React, { useState, useEffect, useRef } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  FormControl,
  FormLabel,
  Input,
  Button,
  VStack,
  useToast,
  useColorModeValue,
  Box,
  Image,
  Text,
} from '@chakra-ui/react';
import BlogService from 'services/BlogService';
import sanitizeHtml from 'sanitize-html';
import BlogEditor from './BlogEditor';

export default function BlogForm({ isOpen, onClose, reloadBlogs, blog }) {
  const toast = useToast();

  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [thumbnail, setThumbnail] = useState('');
  const [loading, setLoading] = useState(false);

  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  const fileInputRef = useRef(null);

  useEffect(() => {
    if (isOpen) {
      if (blog) {
        setTitle(blog.title || '');
        setContent(blog.content || '');
        setThumbnail(blog.thumbnail || '');
      } else {
        setTitle('');
        setContent('');
        setThumbnail('');
      }
    }
  }, [blog, isOpen]);

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onloadend = () => {
      setThumbnail(reader.result);
    };
    reader.readAsDataURL(file);
  };

  const handleSubmit = async (e) => {
    if (e && e.preventDefault) e.preventDefault();
    setLoading(true);

    try {
      const sanitizedContent = sanitizeHtml(content, {
        allowedTags: sanitizeHtml.defaults.allowedTags.concat([
          'img',
          'video',
          'iframe',
        ]),
        allowedAttributes: {
          ...sanitizeHtml.defaults.allowedAttributes,
          img: ['src', 'alt', 'width', 'height'],
          video: ['src', 'width', 'height', 'controls'],
          iframe: [
            'src',
            'width',
            'height',
            'frameborder',
            'allow',
            'allowfullscreen',
          ],
          a: ['href', 'target', 'rel'],
        },
        allowedSchemes: ['http', 'https', 'mailto', 'data'],
      });

      const data = { title, content: sanitizedContent, thumbnail };
      if (blog) {
        await BlogService.updateblog(blog.id, data);
        toast({
          title: 'blog updated successfully',
          status: 'success',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
        });
      } else {
        await BlogService.createblog(data);
        toast({
          title: 'blog created successfully',
          status: 'success',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
        });
      }

      if (reloadBlogs) reloadBlogs();
      onClose();
    } catch (err) {
      console.error(err);
      toast({
        title: 'Error saving blog',
        status: 'error',
        duration: 3000,
        isClosable: true,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered size="xl">
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader bg={headerBg} borderTopRadius="20px">
          {blog ? 'Edit blog' : 'Create New blog'}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <VStack spacing={4} align="flex-start">
            <FormControl>
              <FormLabel>Thumbnail Image</FormLabel>

              <Input
                type="file"
                accept="image/*"
                onChange={handleFileChange}
                display="none"
                ref={fileInputRef}
              />

              <Box
                onClick={() => fileInputRef.current.click()}
                onDrop={(e) => {
                  e.preventDefault();
                  const file = e.dataTransfer.files[0];
                  if (file) {
                    const reader = new FileReader();
                    reader.onloadend = () => {
                      setThumbnail(reader.result);
                    };
                    reader.readAsDataURL(file);
                  }
                }}
                onDragOver={(e) => e.preventDefault()}
                border="2px dashed"
                borderColor={useColorModeValue('gray.300', 'gray.600')}
                borderRadius="md"
                cursor="pointer"
                textAlign="center"
                p={6}
                minH="100px"
                display="flex"
                alignItems="center"
                justifyContent="center"
                flexDirection="column"
                color={useColorModeValue('gray.500', 'gray.400')}
              >
                {thumbnail ? (
                  <Image
                    src={`${thumbnail}`}
                    alt="Thumbnail Preview"
                    maxH="140px"
                    objectFit="contain"
                    borderRadius="md"
                  />
                ) : (
                  <>
                    <Text fontSize="lg" fontWeight="semibold">
                      Click or drag file to upload
                    </Text>
                  </>
                )}
              </Box>
            </FormControl>
            <FormControl isRequired>
              <FormLabel>Title</FormLabel>
              <Input
                color={textColor}
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                placeholder="Enter blog title"
                name="blog-title"
              />
            </FormControl>

            <FormControl isRequired>
              <FormLabel>Content</FormLabel>
              <BlogEditor value={content} onChange={setContent} />
            </FormControl>
          </VStack>
        </ModalBody>

        <ModalFooter bg={headerBg} borderBottomRadius="20px" mt={10}>
          <Button variant="ghost" mr={3} onClick={onClose} type="button">
            Cancel
          </Button>
          <Button
            colorScheme="blue"
            type="submit"
            form="blog-form"
            isLoading={loading}
            onClick={handleSubmit}
          >
            {blog ? 'Update' : 'Create'}
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
