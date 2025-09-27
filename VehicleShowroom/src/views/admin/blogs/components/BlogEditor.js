import React, { useMemo, useCallback, useRef } from 'react';
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';

export default function BlogEditor({ value, onChange }) {
  const quillRef = useRef(null);

  const imageHandler = useCallback(() => {
    const input = document.createElement('input');
    input.setAttribute('type', 'file');
    input.setAttribute('accept', 'image/*');
    input.click();

    input.onchange = () => {
      const file = input.files[0];
      if (!file) return;

      const reader = new FileReader();
      reader.onload = () => {
        const quill = quillRef.current?.getEditor();
        const range = quill.getSelection();
        const position = range ? range.index : quill.getLength();
        quill.insertEmbed(position, 'image', reader.result);
        quill.setSelection(position + 1);
      };
      reader.readAsDataURL(file);
    };
  }, []);

  const modules = useMemo(
    () => ({
      toolbar: {
        container: [
          [{ header: [1, 2, 3, false] }],
          ['bold', 'italic', 'underline', 'strike'],
          [{ list: 'ordered' }, { list: 'bullet' }],
          ['link', 'image', 'video'],
          ['clean'],
        ],
        handlers: { image: imageHandler },
      },
    }),
    [imageHandler],
  );

  const formats = useMemo(
    () => [
      'header',
      'bold',
      'italic',
      'underline',
      'strike',
      'list',
      'bullet',
      'link',
      'image',
      'video',
    ],
    [],
  );

  return (
    <ReactQuill
      ref={quillRef}
      theme="snow"
      value={value}
      onChange={onChange}
      modules={modules}
      formats={formats}
      style={{ height: '200px' }}
    />
  );
}
