import { invoke } from "@tauri-apps/api/tauri";
import { useEffect, useState } from "react";

interface Props {
  displayName: string;
  serviceName: string;
}

const Service = ({ displayName, serviceName }: Props) => {
  const [status, setStatus] = useState<null | boolean>(null);

  useEffect(() => {
    const checkServiceStatus = async () => {
      setStatus(await invoke("check_service", { serviceName }));
    };
    checkServiceStatus();
  }, []);

  let displayStatus;
  if (status === null) {
    displayStatus = "loading";
  } else {
    displayStatus = status ? "running" : "not running";
  }

  return (
    <div className="service-status-container">
      <div className="service-name">{displayName}</div>
      <div className="service-status">{displayStatus}</div>
    </div>
  );
};

export default Service;
