import {
  createStyles,
  Text,
  Group,
  ActionIcon,
  rem,
  Button,
  Modal,
  Flex,
  Avatar,
} from '@mantine/core';
import {
  IconBrandReact,
  IconBrandTypescript,
  IconBrandRust,
  IconBrandDocker,
  IconBrandAws,
  IconBrandCloudflare,
  IconBrandGithub,
} from '@tabler/icons-react';
import { MantineLogo } from '@mantine/ds';
import { useDisclosure } from '@mantine/hooks';
import AnsibleIcon from '../../art/ansibleIcon.svg';

const useStyles = createStyles((theme) => ({
  footer: {
    width: '100%',
    display: 'flex',
    justifyContent: 'center',
    borderTop: `${rem(1)} solid ${
      theme.colorScheme === 'dark' ? theme.colors.dark[5] : theme.colors.gray[2]
    }`,
  },
  modalItems: {
    display: 'flex',
    justifyContent: 'flex-end',
    alignContent: 'left',
    alignItems: 'left',
    flexDirection: 'column',
  },
}));
interface Credits {
  title: string;
  content: { name: string; icon: JSX.Element | string }[];
}
const credits: Credits[] = [
  {
    title: 'Frontend',
    content: [
      { name: 'React', icon: <IconBrandReact /> },
      { name: 'Typescript', icon: <IconBrandTypescript /> },
      { name: 'Mantine', icon: <MantineLogo type="mark" size={rem(25)} /> },
    ],
  },
  {
    title: 'Backend',
    content: [{ name: 'Rust', icon: <IconBrandRust /> }],
  },
  {
    title: 'DevOps',
    content: [
      { name: 'Docker', icon: <IconBrandDocker /> },
      { name: 'Ansible', icon: <Avatar src={AnsibleIcon} size={rem(25)} /> },
    ],
  },
  {
    title: 'Server',
    content: [{ name: 'AWS', icon: <IconBrandAws /> }],
  },
  {
    title: 'Domain',
    content: [{ name: 'Cloudflare', icon: <IconBrandCloudflare /> }],
  },
];

export const Footer = () => {
  const { classes } = useStyles();
  const [opened, { open, close }] = useDisclosure(false);

  return (
    <div className={classes.footer}>
      <Modal
        radius={15}
        opened={opened}
        onClose={close}
        title="Credits"
        centered>
        <Flex direction="column">
          <Group>
            <Text>This site was developed by me using:</Text>
          </Group>
          {credits.map((credit) => (
            <Group key={credit.title} mt={10}>
              <Text style={{ width: '100%' }} variant="h4" weight={700}>
                {credit.title}:
              </Text>
              <Group mt={5}>
                {credit.content.map((item) => (
                  <Group key={item.name}>
                    <ActionIcon
                      variant="outline"
                      color="violet"
                      radius="xl"
                      style={{ width: rem(40), height: rem(40) }}
                      title={item.name}
                      aria-label={item.name}>
                      {item.icon}
                    </ActionIcon>
                    <Text size="sm">{item.name}</Text>
                  </Group>
                ))}
              </Group>
            </Group>
          ))}
          <Group mt={20}>
            <Text>Repo link: </Text>
            <Button
              variant="default"
              color="violet"
              component="a"
              target="_blank"
              rel="noopener noreferrer"
              href={'https://github.com/raygamedev/raydevsrs'}
              radius="xl"
              title="GitHub"
              aria-label="GitHub"
              onClick={close}
              leftIcon={<IconBrandGithub />}>
              GitHub
            </Button>
          </Group>
        </Flex>
      </Modal>
      <Button onClick={open} mt={10} variant="subtle" color="violet">
        Credits
      </Button>
    </div>
  );
};
