import { Status } from "../common/ServiceStatus";

interface Props {
  state: Status;
}

const StatusDot = ({ state }: Props) => {
  let s = state.replace(" ", "-");
  return <div className={`status-dot status-dot-${s}`}></div>;
};

export default StatusDot;
