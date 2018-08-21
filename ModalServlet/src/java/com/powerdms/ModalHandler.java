package com.powerdms;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.FilenameUtils;
import org.apache.commons.io.IOUtils;

import java.io.File;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Collections;

public class ModalHandler {
    private String path;
    private String out;

    public boolean load(){
        if(!loadResource(Constants.COMMAND_LINE_LIB))
            return false;
        if(!loadResource(Constants.MODAL_HANDLER_LIB))
            return false;
        setOut(Messages.HANDLER_PATH + path, true);
        if(!loadResource(Constants.MODAL_HANDLER_CONSOLE))
            return false;
        setOut(Messages.CONSOLE_PATH + path, true);
        return true;
    }

    private boolean loadResource(String name){
        try{
            // Get resource by name.
            URL resourcePath = getClass().getResource("/" + name);
            // Get currently executing directory's parent.
            String baseDir = new File(getClass().getProtectionDomain().getCodeSource().getLocation().getFile()).getParent();
            // Destination resource file.
            File dest = new File(baseDir + "\\" + name);
            // Copy the resource into the destination file.
            FileUtils.copyURLToFile(resourcePath, dest);
            path = dest.getCanonicalPath();
            // File is to be removed when JVM exists, i.e. GRID node is stopped.
            dest.deleteOnExit();
            return Files.exists(Paths.get(path));
        }catch (Exception e){
            printStacktrace(e);
            return false;
        }
    }

    public synchronized int exec(String[] args) throws Exception {
        try {
            Runtime rt = Runtime.getRuntime();
            ArrayList<String> commands = new ArrayList<String>();
            commands.add(path);
            Collections.addAll(commands, args);
            Process process = rt.exec(commands.toArray(new String[0]));
            setOut(IOUtils.toString(process.getInputStream()), false);
            return process.exitValue();
        } catch (Exception e) {
            printStacktrace(e);
            throw new Exception(e);
        }
    }

    private void printStacktrace(Exception e) {
        StringWriter sw = new StringWriter();
        e.printStackTrace(new PrintWriter(sw));
        setOut(sw.toString(), true);
    }

    public String getOut(){
        return out;
    }

    void clearOut(){
        out = null;
    }

    private void setOut(String str, boolean addTimestamp) {
        out =  Util.writeLine(out, str, addTimestamp);
    }
}
