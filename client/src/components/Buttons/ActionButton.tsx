import { Button } from '@mantine/core';

interface Props {
  text: string;
  variant: 'default' | 'outline' | 'light' | 'filled' | 'link';
  onClick: () => void;
}
export const ActionButton = ({ text, variant, onClick }: Props) => {
  return (
    <div>
      <Button color="violet" variant={variant} onClick={onClick}>
        {text}
      </Button>
    </div>
  );
};
